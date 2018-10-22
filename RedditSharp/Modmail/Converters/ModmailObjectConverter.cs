using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditSharp.Modmail.Converters
{
    public abstract class ModmailObjectConverter : JsonConverter
    {
        public IWebAgent WebAgent { get; set; }

        public ModmailObjectConverter()
        {
            ;
        }

        public ModmailObjectConverter(IWebAgent agent)
        {
            if (agent == null)
                throw new Exception("null");

            WebAgent = agent;
        }
    }
}
