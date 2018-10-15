using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RedditSharp.Things;

namespace RedditSharp.Modmail
{
    public class RecentCollection<T> : IAsyncEnumerable<T> where T : RedditObject
    {
        internal IWebAgent WebAgent;
        internal string[] Urls;

        public RecentCollection(IWebAgent webAgent, string[] urls)
        {
            WebAgent = webAgent;
            Urls = urls;
        }

        /// <inheritdoc />
        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new RecentCollectionEnumerator<T>(this);
        }
    }

    public class RecentCollectionEnumerator<T> : IAsyncEnumerator<T> where T : RedditObject
    {
        private IWebAgent wa;
        private string[] urls;
        private int index = 0;

        public RecentCollectionEnumerator(RecentCollection<T> recentComments)
        {
            wa = recentComments.WebAgent;
            urls = recentComments.Urls;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            //
        }

        /// <inheritdoc />
        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var max = urls.GetUpperBound(0);
            if (index == max)
                return false;

            var url = urls[index].Replace("www.reddit.com/", "oauth.reddit.com/") + ".json";

            Current = RedditObject.Parse<T>(wa, await wa.Get(url));

            return true;
        }

        /// <inheritdoc />
        public T Current { get; private set; }
    }

}
