using System;
using Newtonsoft.Json;

namespace RedditSharp.Modmail
{
    public class Message : ModmailObject
    {
        [JsonProperty]
        public string Body { get; set; }

        [JsonProperty]
        public string BodyMarkdown { get; set; }

        [JsonProperty]
        public Author Author { get; set; }

        [JsonProperty]
        public bool IsInternal { get; set; }

        [JsonProperty]
        public DateTime Date { get; set; }

        /// <inheritdoc />
        public Message(IWebAgent agent) : base(agent)
        {
        }
    }
}

/*
{
   "body" : "<!-- SC_OFF --><div class=\"md\"><p>+1</p>\n</div><!-- SC_ON -->",
   "author" : {
      "isMod" : true,
      "isAdmin" : false,
      "name" : "foobar",
      "isOp" : false,
      "isParticipant" : false,
      "isHidden" : false,
      "id" : 12345678,
      "isDeleted" : false
   },
   "isInternal" : true,
   "date" : "2018-10-11T23:06:09.615991-04:00",
   "bodyMarkdown" : "+1",
   "id" : "8klo8"
}

 */
