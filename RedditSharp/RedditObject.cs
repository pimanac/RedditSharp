using Newtonsoft.Json;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using RedditSharp.Modmail;

namespace RedditSharp
{
    /// <summary>
    /// Wrapper class to provide <see cref="IWebAgent"/> to children.
    /// </summary>
    public abstract class RedditObject
    {
        /// <summary>
        /// WebAgent for requests
        /// </summary>
        [JsonIgnore]
        public IWebAgent WebAgent { get; }

        /// <summary>
        /// Assign <see cref="WebAgent"/>
        /// </summary>
        /// <param name="agent"></param>
        public RedditObject(IWebAgent agent)
        {
            WebAgent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public static T Parse<T>(IWebAgent agent, JToken json) where T : RedditObject
        {
            RedditObject result;

            if (typeof(T).GetTypeInfo().IsAssignableFrom(typeof(ModmailObject).GetTypeInfo()))
                result = null;
            else
                result = RedditSharp.Things.Thing.Parse(agent, json);

            return result as T;
        }

    }

}