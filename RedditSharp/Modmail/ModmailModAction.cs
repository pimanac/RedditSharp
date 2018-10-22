using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditSharp.Modmail.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditSharp.Modmail
{
    public class ModmailModAction : ModmailObject
    {
        [JsonProperty("date")]
        public DateTime? Date { get; private set; }

        [JsonProperty("actionTypeId")]
        public int ActionTypeId { get; private set; }

        [JsonProperty("author")]
        public Author Author { get; private set; }

        public ModmailModAction(IWebAgent agent, JToken json) : base(agent, json)
        {

        }
    }
}
