#pragma warning disable 1591
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedditSharp.Modmail;
using RedditSharp.Things;

namespace RedditSharp.Extensions
{
    public static class Extensions
    {
        public static T ValueOrDefault<T>(this IEnumerable<JToken> enumerable)
        {
            if (enumerable == null)
                return default(T);
            return enumerable.Value<T>();
        }

        /// <summary>
        /// Get the <see cref="RedditSharp.Things.RedditUser">RedditUser</see> associated with this Author.
        /// </summary>
        /// <param name="author">Modmail <see cref="RedditSharp.Modmail.Author"/>Author.</param>
        /// <param name="webAgent">An active <see cref="RedditSharp.IWebAgent"/>WebAgent.</param>
        /// <returns></returns>
        public static async Task<RedditSharp.Things.RedditUser> GetUserAsync(this RedditSharp.Modmail.Author author,IWebAgent webAgent)
        {
            return await RedditUser.GetUserAsync(webAgent, author.Name).ConfigureAwait(false);
        }
    }
}
#pragma warning restore 1591