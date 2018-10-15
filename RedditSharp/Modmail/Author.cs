using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RedditSharp.Things;

namespace RedditSharp.Modmail
{
    /*
     * 
     * {
            "isMod" : true,
            "isAdmin" : false,
            "name" : "foobar",
            "isOp" : false,
            "isParticipant" : false,
            "isHidden" : false,
            "id" : 123456789,
            "isDeleted" : false
         }, {
            "isMod" : false,
            "isAdmin" : false,
            "name" : "barfoo",
            "isOp" : true,
            "isParticipant" : true,
            "isHidden" : false,
            "id" : 987654321,
            "isDeleted" : false
         },
     * 
     */
    public class Author : ModmailObject
    {
        /// <summary>
        /// Indiciates if the user is a moderator for the subreddit of this conversation.
        /// </summary>
        [JsonProperty]
        public bool IsMod { get; set; }

        /// <summary>
        /// Indicates if this user is a reddit admin.
        /// </summary>
        [JsonProperty]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Reddit username
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// Indicates if this author is person who initiated the conversation.
        /// </summary>
        [JsonProperty]
        public bool IsOp { get; set; }

        /// <summary>
        /// Indiciates if this author is a participant of this conversation.
        /// </summary>
        [JsonProperty]
        public bool IsParticipant { get; set; }

        /// <summary>
        /// I don't know what this means
        /// </summary>
        [JsonProperty]
        public bool IsHidden { get; set; }

        /// <summary>
        /// Indicates if this author has a deleted account?
        /// </summary>
        [JsonProperty]
        public bool IsDeleted { get; set; }

        /// <inheritdoc />
        public Author(IWebAgent agent) : base(agent)
        {
        }
    }
}
