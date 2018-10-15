using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace RedditSharp.Modmail
{
    public class Conversation : ModmailObject
    {
        [JsonProperty]
        public bool IsAuto { get; set; }

        [JsonProperty]
        public bool IsRepliable { get; set; }

        [JsonProperty]
        public DateTime LastUserUpdate { get; set; }

        [JsonProperty]
        public bool IsInternal { get; set; }

        [JsonProperty]
        public DateTime LastModUpdate { get; set; }

        [JsonProperty]
        public DateTime LastUpdated { get; set; }

        [JsonIgnore] // we will read this in the converter
        public string Subreddit { get; set; }

        [JsonIgnore] // we will read this in the converter
        public string SubredditId { get; set; }

        [JsonProperty]
        public bool IsHighlighted { get; set; }

        [JsonProperty]
        public string Subject { get; set; }

        [JsonProperty]
        public Author Participant { get; set; }

        [JsonProperty]
        public string State { get; set; }

        [JsonProperty]
        public DateTime LastUnread { get; set; }

        [JsonProperty]
        public int NumMessages { get; set; }

        [JsonProperty]
        public ModmailEntityCollection<Author> Authors { get; internal set; }

        public ModmailEntityCollection<Message> Messages { get; internal set; }


        public Conversation(IWebAgent agent) : base(agent)
        {
            this.Authors = new ModmailEntityCollection<Author>(new List<Author>());
        }
    }
}

/*


{
   "isAuto" : false,
   "objIds" : [{
         "id" : "8kngb",
         "key" : "messages"
      }
   ],
   "isRepliable" : true,
   "lastUserUpdate" : "2018-10-12T01:17:22.664472-04:00",
   "isInternal" : false,
   "lastModUpdate" : "2018-10-12T01:20:49.397535-04:00",
   "lastUpdated" : "2018-10-12T01:20:49.397535-04:00",
   "authors" : [{
         "isMod" : true,
         "isAdmin" : false,
         "name" : "foobar",
         "isOp" : false,
         "isParticipant" : false,
         "isHidden" : false,
         "id" : 874654321,
         "isDeleted" : false
      }, {
         "isMod" : false,
         "isAdmin" : false,
         "name" : "barfoo",
         "isOp" : false,
         "isParticipant" : true,
         "isHidden" : false,
         "id" : 12345678,
         "isDeleted" : false
      }
   ],
   "owner" : {
      "displayName" : "foobar",
      "type" : "subreddit",
      "id" : "t5_xxxxx"
   },
   "id" : "5bsr0",
   "isHighlighted" : true,
   "subject" : "Hello, world.",
   "participant" : {
      "isMod" : false,
      "isAdmin" : false,
      "name" : "barfoo",
      "isOp" : false,
      "isParticipant" : true,
      "isHidden" : true,
      "id" : 12345678,
      "isDeleted" : false
   },
   "state" : 1,
   "lastUnread" : "2018-10-12T01:17:22.67647-04:00",
   "numMessages" : 5
}

 */
