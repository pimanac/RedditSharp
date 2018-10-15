using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditSharp;
using RedditSharp.Things;

namespace RedditSharp.Modmail
{
    public class ModmailUser : ModmailObject
    {
        internal RecentCollection<Conversation> _recentConvos;
        internal RecentCollection<Comment> _recentComments;
        internal RecentCollection<Post> _recentPosts;

        [JsonProperty]
        public UserMuteStatus MuteStatus { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public DateTime DateCreated { get; set; }

        [JsonProperty]
        public UserBanStatus BanStatus { get; set; }

        [JsonProperty]
        public bool IsSuspended { get; set; }

        [JsonProperty]
        public bool IsShadowbanned { get; set; }

        public IAsyncEnumerable<Comment> GetRecentComments() => _recentComments;
        public IAsyncEnumerable<Conversation> GetRecentConversations() => _recentConvos;
        public IAsyncEnumerable<Post> GetRecentPosts() => _recentPosts;

        public async Task<RedditUser> GetRedditUser()
        {
            var json = await WebAgent.Get($"https://oauth.reddit.com/user/{Name}.json").ConfigureAwait(false);
            return Thing.Parse<RedditUser>(WebAgent, json);
        }

        /// <inheritdoc />
        public ModmailUser(IWebAgent agent) : base(agent)
        {

        }
    }

    public class UserMuteStatus
    {
        [JsonProperty]
        public bool IsMuted { get; set; }
        [JsonProperty]
        public DateTime? EndDate { get; set; }
        [JsonProperty]
        public string Reason { get; set; }
    }

    public class UserBanStatus
    {
        [JsonProperty]
        public DateTime? EndDate { get; set; }
        [JsonProperty]
        public string Reason { get; set; }
        [JsonProperty]
        public bool IsBanned { get; set; }
        [JsonProperty]
        public bool IsPermanent { get; set; }
    }
}


