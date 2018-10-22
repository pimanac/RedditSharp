using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditSharp.Modmail.Converters
{
#pragma warning disable 1591
    internal class ConversationConverter : ModmailObjectConverter
    {
        public ConversationConverter(IWebAgent agent) : base(agent)
        {
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var json = JObject.Load(reader);
            var xx = json.ToString();


            var result = new Conversation(WebAgent, json);
            serializer.Populate(json.CreateReader(), result);

            try
            {
                result.Subreddit = json["owner"]["displayName"].Value<string>();
                result.SubredditId = json["owner"]["id"].Value<string>().Split('_')[1];
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

    }
#pragma warning restore 1591
}