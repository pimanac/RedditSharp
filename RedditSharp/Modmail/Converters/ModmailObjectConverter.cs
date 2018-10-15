using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedditSharp.Modmail.Converters
{
    public abstract class ModmailObjectConverter : JsonConverter
    {
        public IWebAgent WebAgent { get; set; }

        protected ModmailObjectConverter(IWebAgent agent)
        {
            WebAgent = agent;
        }
    }
}
