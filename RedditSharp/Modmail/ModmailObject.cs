using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditSharp.Extensions;
using RedditSharp.Modmail.Converters;

namespace RedditSharp.Modmail
{
    public class ModmailObject : RedditObject
    {
        [JsonProperty]
        public string Id { get; set; }

        public JToken RawJson { get; set; }

        /// <summary>
        /// Gets a property of this Thing, without any automatic conversion.
        /// </summary>
        /// <param name="property">The reddit API name of the property</param>
        /// <returns>The property's value as a <see cref="String"/> or null if the property 
        /// doesn't exist or is null.</returns>
        public string this[string property] => RawJson[property].ValueOrDefault<string>();

        /// <inheritdoc />
        public ModmailObject(IWebAgent agent, JToken json) : base(agent)
        {
            RawJson = json;
        }
    }

}
