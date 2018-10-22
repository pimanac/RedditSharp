using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace RedditSharp.Modmail.Converters
{
    public class ModmailModActionConverter : ModmailObjectConverter
    {
        public ModmailModActionConverter(IWebAgent agent) : base(agent)
        {
            WebAgent = agent;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ModmailModAction);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var json = JObject.Load(reader);
            var result = new ModmailModAction(WebAgent, json);
            
            serializer.Populate(reader, result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
