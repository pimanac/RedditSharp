using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditSharp.Modmail
{
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
        public Author(IWebAgent agent, JToken json) : base(agent, json)
        {
        }
    }
}
