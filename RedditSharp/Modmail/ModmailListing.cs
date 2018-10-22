using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedditSharp.Modmail
{
    public sealed class ModmailListing : IListing<Conversation>
    {
        public IWebAgent WebAgent { get; }
        public string Url { get; set; }

        //Get conversations for a logged in user or subreddits
        //    after

        //base36 modmail conversation id
        //entity

        //    comma-delimited list of subreddit names
        //    limit //an integer(default: 25)
        //sort

        //    one of(recent, mod, user, unread)
        //state

        //    one of(new, inprogress, mod, notifications, archived, highlighted, all)

        public ModmailListing(IWebAgent agent, string url, int max = -1, int perRequest = 100)
        {
            WebAgent = agent;
            Url = url;
            MaximumLimit = max;
            LimitPerRequest = perRequest;
        }

        internal static ModmailListing Create(IWebAgent agent, string url, int max = -1, int perRequest = -1)
        {
            if (max > 0 && max <= perRequest)
                perRequest = max;

            return new ModmailListing(agent, url, max);
        }

        /// <inheritdoc />
        public IAsyncEnumerator<Conversation> GetEnumerator()
        {
            return new ModmailListingEnumerator(this,LimitPerRequest,MaximumLimit);
        }

        /// <inheritdoc />
        public int LimitPerRequest { get; set; }

        /// <inheritdoc />
        public int MaximumLimit { get; set; }

        /// <inheritdoc />
        public bool IsStream { get; set; }

        public enum ModmailState
        {
            @New, InProgress, Mod, Notifications, Archived, Highlighted, All
        }

        public enum ModmailSort
        {
            Recent, Mod, User, Unread
        }
    }

    public sealed class ModmailListingEnumerator : IAsyncEnumerator<Conversation> {

        private string _before;
        private string _after;
        private int _limitPerRequest = 100;
        private int _count;
        private int _currentIndex = -1;
        private int _max;
        private bool _stream;
        private int _lastCount;

        private ModmailListing _listing;

        private ModmailObjectCollection<Conversation> _conversations;
        private ModmailObjectCollection<Message> _messages;

        private string AppendQueryParam(string url, string param, string value) =>
                url + (url.Contains("?") ? "&" : "?") + param + "=" + value;


        public ModmailListingEnumerator(ModmailListing listing, int limitPerRequest, int maximumLimit, bool stream = false)
        {
            _listing = listing;
            _limitPerRequest = limitPerRequest;
            _max = maximumLimit;
            _stream = stream;
            _conversations = new ModmailObjectCollection<Conversation> ();
            _messages = new ModmailObjectCollection<Message> ();

        }

        /// <inheritdoc />
        public Conversation Current => _conversations[_currentIndex];

        private async Task PageAsync()
        {
            var url = _listing.Url;
            if (_after != null)
            {
                url = AppendQueryParam(url, "after", _after);
            }
            url = AppendQueryParam(url,"limit",_limitPerRequest.ToString());
            var json = await _listing.WebAgent.Get(url).ConfigureAwait(false);
            
            Parse(json);
        }


        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_currentIndex == -1)
            {
                //first call, get a page and set CurrentIndex
                await PageAsync().ConfigureAwait(false);
                _currentIndex = 0;
                return _conversations.Count > 0; //if there are no results, return false
            }

            _count++;
            _currentIndex++;

           
            
            //I don't think we want to use Count here. Look into this.
            if (_max != -1 && _count >= _max)
            {
                // Maximum listing count returned
                return false;
            }
            if (_currentIndex >= _conversations.Count)
            {
                // We've reached 
                if (_after == null)
                {
                    // No more pages to return
                    return false;
                }
                else
                {
                    await PageAsync().ConfigureAwait(false);
                    return _count < _conversations.Count; //if there are no results, return false
                }
            }
            _lastCount = _count;
            return true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            //
        }

        private void Parse(JToken json)
        {
            var messages = (JObject)json["messages"];
            var convos = (JObject)json["conversations"];
            if (convos.Count == 0)
            {
                /// nothing more to get
                _after = null;
                return;
            }
            var convoIds = json["conversationIds"].Select(j => j.Value<string>()); 


            foreach (var item in messages.Properties())
            {
                var message = messages[item.Name].ToObject<Message>();
                _messages.Add(message);
            }

            foreach (var item in convoIds)
            {
                
                var convo = convos[item].ToObject<Conversation>();


                // get the keys of messages associated with this convo
                foreach (var i in convos[item]["objIds"].Children())
                {
                    var key = i["key"].Value<string>();
                    var id = i["id"].Value<string>();
                    switch (key)
                    {
                        case "messages":
                            convo.Messages.Add(_messages[id]);
                            break;
                        case "modActions":
                            break;
                        default:
                            continue; // add more as needed
                    }
                }
                _conversations.Add(convo);
            }


            // What's the last one we've seen?
            var after = _conversations[_conversations.Count - 1].Id;

            if (_after == after)
                // We've seen this before, set to null to prevent fetching another page.
                _after = null;
            else
                _after = after;

        }
    }
}
