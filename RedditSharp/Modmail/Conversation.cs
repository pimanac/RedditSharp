using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditSharp.Modmail.Converters;

namespace RedditSharp.Modmail
{
    public class Conversation : ModmailObject
    {
        internal const string ENDPOINT = "/api/mod/conversations";

        [JsonProperty("isAuto")]
        public bool IsAuto { get; set; }

        [JsonProperty("isRepliable")]
        public bool IsRepliable { get; set; }

        [JsonProperty("lastUserUpdate")]
        public DateTime? LastUserUpdate { get; set; }

        [JsonProperty("isInternal")]//
        public bool IsInternal { get; set; }

        [JsonProperty("lastModUpdate")]
        public DateTime? LastModUpdate { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime? LastUpdated { get; set; }

        [JsonIgnore] // we will read this in the converter
        public string Subreddit { get; set; }

        [JsonIgnore] // we will read this in the converter
        public string SubredditId { get; set; }

        [JsonProperty("isHighlighed")]
        public bool IsHighlighted { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("participant")]
        public Author Participant { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("lastUnread")]
        public DateTime? LastUnread { get; set; }

        [JsonProperty("numMessages")]
        public int MessageCount { get; set; }

        [JsonProperty("authors")]
        public ICollection<Author> Authors { get; internal set; }

        [JsonIgnore()]
        public ModmailObjectCollection<Message> Messages { get; internal set; }

        /// <summary>
        /// Fetches the latest version of the conversation and adds any new comments/action to the
        /// collection.
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAsync()
        {
            // what's the last
            var url = $"{ENDPOINT}/{this.Id}";
            var json = await WebAgent.Get(url).ConfigureAwait(false);
            RawJson = json;
            var messages = json["messages"];

            foreach (var item in json["conversation"]["objIds"].Children())
            {
                var key = item["key"].Value<string>();
                var id = item["id"].Value<string>();
                switch (key)
                {
                    case "messages":
                        if (this.Messages.Contains(id))
                            continue;
                        try
                        {
                            var m = JsonConvert.DeserializeObject<Message>(json["messages"][id].ToString());
                            this.Messages.Add(m);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "modActions":
                        break;
                    default:
                        continue; // add more as needed
                }
            }
        }

        /// <summary>
        /// Archive this conversation.
        /// </summary>
        /// <returns></returns>
        public async Task ArchiveAsync()
        {
            await WebAgent.Post($"{ENDPOINT}/{Id}/archive", new
            {
                conversation_id = Id
            });
        }

        /// <summary>
        /// Unarchive this conversation.
        /// </summary>
        /// <returns></returns>
        public async Task UnArchiveAsync()
        {
            await WebAgent.Post($"{ENDPOINT}/{Id}/unarchive", new
            {
                conversation_id = Id
            });
        }

        /// <summary>
        /// Mutes the conversation.  Non-moderators can not reply for 72 hours.
        /// </summary>
        /// <returns></returns>
        public async Task MuteAsync()
        {
            await WebAgent.Post($"{ENDPOINT}/{Id}/mute", new
            {
                conversation_id = Id
            });
        }
    
        /// <summary>
        /// Unmutes the conversation.
        /// </summary>
        /// <returns></returns>
        public async Task UnMuteAsync()
        {
            await WebAgent.Post($"{ENDPOINT}/{Id}/unmute", new
            {
                conversation_id = Id
            });
        }

        /// <summary>
        /// Reply to the conversation.
        /// </summary>
        /// <param name="body">message body.</param>
        /// <param name="internal">Mark the message as internal.</param>
        /// <param name="authorHidden">hide the message from non-moderators.</param>
        /// <returns></returns>
        public async Task NewMessage(string body, bool @internal = false, bool authorHidden = false)
        {
            await WebAgent.Post($"{ENDPOINT}/{Id}", new
            {
                body = body,
                conversation_id = Id,
                isInternal = @internal,
                isAuthorHidden = authorHidden
            });
        }

        /// <summary>
        /// Highlight this conversation.
        /// </summary>
        /// <returns></returns>
        public async Task HighlightAsync()
        {
            await WebAgent.Post($"{ENDPOINT}/{Id}/highlight", new
            {
                conversation_id = Id
            });
        }


        /// <summary>
        /// Remove the highlight from this conversation.
        /// </summary>
        /// <returns></returns>
        public async Task UnHighlightAsync()
        {
            var req = WebAgent.CreateRequest($"{ENDPOINT}/{Id}/highlight", "DELETE");
            WebAgent.WritePostBody(req, new
            {
                conversation_id = Id
            });

            await WebAgent.ExecuteRequestAsync(() => req);


        }



        public Conversation(IWebAgent agent, JToken json) : base(agent, json)
        {
            this.Authors = new List<Author>();
            this.Messages = new ModmailObjectCollection<Message>();
        }
        
    }
}

/*


{
   "isAuto" : false,
   "objIds" : [{
         "id" : "8kngb",
         "key" : "messages"
      }
   ],
   "isRepliable" : true,
   "lastUserUpdate" : "2018-10-12T01:17:22.664472-04:00",
   "isInternal" : false,
   "lastModUpdate" : "2018-10-12T01:20:49.397535-04:00",
   "lastUpdated" : "2018-10-12T01:20:49.397535-04:00",
   "authors" : [{
         "isMod" : true,
         "isAdmin" : false,
         "name" : "foobar",
         "isOp" : false,
         "isParticipant" : false,
         "isHidden" : false,
         "id" : 874654321,
         "isDeleted" : false
      }, {
         "isMod" : false,
         "isAdmin" : false,
         "name" : "barfoo",
         "isOp" : false,
         "isParticipant" : true,
         "isHidden" : false,
         "id" : 12345678,
         "isDeleted" : false
      }
   ],
   "owner" : {
      "displayName" : "foobar",
      "type" : "subreddit",
      "id" : "t5_xxxxx"
   },
   "id" : "5bsr0",
   "isHighlighted" : true,
   "subject" : "Hello, world.",
   "participant" : {
      "isMod" : false,
      "isAdmin" : false,
      "name" : "barfoo",
      "isOp" : false,
      "isParticipant" : true,
      "isHidden" : true,
      "id" : 12345678,
      "isDeleted" : false
   },
   "state" : 1,
   "lastUnread" : "2018-10-12T01:17:22.67647-04:00",
   "numMessages" : 5
}

 */
