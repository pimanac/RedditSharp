using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditSharp.Modmail.Converters
{
#pragma warning disable 1591
    public class AuthorConverter : ModmailObjectConverter
    {
        public AuthorConverter(IWebAgent agent) : base(agent)
        {

        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var json = JObject.Load(reader);
            if (WebAgent == null)
                throw new Exception("null?");

            var result = new Author(WebAgent, json);
            serializer.Populate(json.CreateReader(), result);
            return result;
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Author);
        }
    }
#pragma warning restore 1591
}
