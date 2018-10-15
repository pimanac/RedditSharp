using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedditSharp.Modmail
{
    public class ModmailListing : IListing<ModmailObject>
    {
        private IWebAgent _agent;

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
        private const string ENDPOINT = "/api/mod/conversations";


        public ModmailListing(IWebAgent agent, string url, int max = -1)
        {
            _agent = agent;
        }

        internal static ModmailListing Create(IWebAgent agent, string url, int max = -1, int perRequest = -1)
        {
            if (max > 0 && max <= perRequest)
                perRequest = max;

            return new ModmailListing(agent, url, max);
        }

        /// <inheritdoc />
        public IAsyncEnumerator<ModmailObject> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int LimitPerRequest { get; set; }

        /// <inheritdoc />
        public int MaximumLimit { get; set; }

        /// <inheritdoc />
        public bool IsStream { get; set; }
    }

    public class ModmailListingEnumerator : IAsyncEnumerator<ModmailObject> {
        /// <inheritdoc />
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ModmailObject Current { get; }
    }
}
