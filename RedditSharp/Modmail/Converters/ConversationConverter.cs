using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditSharp.Modmail.Converters
{
    internal class ConversationConverter : ModmailObjectConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new Conversation(WebAgent);
            serializer.Populate(reader, result);

            var json = JObject.Load(reader);
            try
            {
                result.Subreddit = json["owner"]["displayName"].Value<string>();
                result.SubredditId = json["owner"]["id"].Value<string>().Split('_')[0];
            }
            catch
            {
                result.Subreddit = null;
            }

            return result;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Conversation);
        }

        /// <inheritdoc />
        public ConversationConverter(IWebAgent agent) : base(agent)
        {
        }
    }
}