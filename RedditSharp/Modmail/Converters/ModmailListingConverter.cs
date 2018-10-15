using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace RedditSharp.Modmail.Converters
{
    public class ModmailListingConverter : ModmailObjectConverter
    {
        public ModmailListingConverter(IWebAgent agent) : base(agent)
        {

        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ModmailListing);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new ModmailListing(WebAgent);
            var json = JObject.Load(reader);

           // if ()
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
