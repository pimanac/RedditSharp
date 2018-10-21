using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace RedditSharp.Things
{
    /// <summary>
    /// A thing that can be voted on or actionable by a moderator.
    /// </summary>
    public class VotableThing : ModeratableThing
    {
        /// <inheritdoc />
        public VotableThing(IWebAgent agent, JToken json) : base(agent, json) {
        }
        /// <summary>
        /// Type of vote to do
        /// </summary>
        public enum VoteType
        {
            /// <summary>
            /// An upvote, adds a vote
            /// </summary>
            Upvote = 1,
            /// <summary>
            /// No vote / removes a vote
            /// </summary>
            None = 0,
            /// <summary>
            /// Downvotes, removes a vote someone else has made (technically)
            /// </summary>
            Downvote = -1
        }

        private const string VoteUrl = "/api/vote";
        private const string SaveUrl = "/api/save";
        private const string UnsaveUrl = "/api/unsave";

        private const string DelUrl = "/api/del";

        /// <summary>
        /// Author flair background color (hex triplet)
        /// </summary>
        [JsonProperty("author_flair_background_color")]
        public string AuthorFlairBackgroundColor { get; private set; }

        /// <summary>
        /// Css flair class of the item author.
        /// </summary>
        [JsonProperty("author_flair_css_class")]
        public string AuthorFlairCssClass { get; private set; }

        /// <summary>
        /// Author flair template id
        /// </summary>
        [JsonProperty("author_flair_template_id")]
        public string AuthorFlairTemplateId { get; private set; }

        /// <summary>
        /// Flair text of the item author.
        /// </summary>
        [JsonProperty("author_flair_text")]
        public string AuthorFlairText { get; private set; }

        /// <summary>
        /// Flair text color of the item author.
        /// </summary>
        [JsonProperty("author_flair_text_color")]
        public string AuthorFlairTextColor { get; private set; }

        /// <summary>
        /// Flair type.
        /// </summary>
        [JsonProperty("author_flair_type")]
        public string AuthorFlairType { get; private set; }

        /// <summary>
        /// The <see cref="RedditSharp.Things.Thing.FullName">fullname</see> of the Author.
        /// </summary>
        [JsonProperty("author_fullname")]
        public string AuthorFullName { get; private set; }

        /// <summary>
        /// Returns true if the user has patreon flair.
        /// </summary>
        [JsonProperty("author_patreon_flair")]
        public bool HasAuthorPatreonFlair { get; private set; }

        /// <summary>
        /// When the item was removed.
        /// </summary>
        [JsonProperty("banned_at_utc")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime? BannedAtUTC { get; private set; }

        /// <summary>
        /// Returns true if reddit gold/premium can be gifted to the author.
        /// </summary>
        [JsonProperty("can_guild")]
        public bool IsGuildable { get; private set; }

        /// <summary>
        /// category - not used?
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; private set; }

        /// <summary>
        /// Number of upvotes on this item.
        /// </summary>
        [JsonProperty("downs")]
        public int Downvotes { get; private set; }

        /// <summary>
        /// Returns true if this item has been edited by the author.
        /// </summary>
        [JsonProperty("edited")]
        public bool Edited { get; private set; }

        /// <summary>
        /// Returns true if this item is archived.
        /// </summary>
        [JsonProperty("archived")]
        public bool IsArchived { get; private set; }
 
        /// <summary>
        /// Number of upvotes on this item.
        /// </summary>
        [JsonProperty("ups")]
        public int Upvotes { get; private set; }

        /// <summary>
        /// Returns true if this item is saved.
        /// </summary>
        [JsonProperty("saved")]
        public bool Saved { get; private set; }

        /// <summary>
        /// Shortlink to the item
        /// </summary>
        public virtual string Shortlink => "http://redd.it/" + Id;

        /// <summary>
        /// The permalink for an item
        /// </summary>
        [JsonProperty("permalink")]
        [JsonConverter(typeof(UrlParser))]
        public Uri Permalink { get; private set; }

        /// <summary>
        /// Returns true if the item is sticked.
        /// </summary>
        [JsonProperty("stickied")]
        public bool IsStickied { get; private set; }

        /// <summary>
        /// True if the logged in user has upvoted this.
        /// False if they have not.
        /// Null if they have not cast a vote.
        /// </summary>
        [JsonProperty("likes")]
        public bool? Liked { get; private set; }

        /// <summary>
        /// Number of times this item has been gilded.
        /// </summary>
        [JsonProperty("gilded")]
        public int Gilded { get; private set; }

        /// <summary>
        /// No follow.
        /// </summary>
        [JsonProperty("no_follow")]
        public bool NoFollow { get; private set; }

        /// <summary>
        /// Subreddit full name.
        /// </summary>
        [JsonProperty("subreddit_id")]
        public string SubredditFullName { get; private set; }

        /// <summary>
        /// Subreddit display name.
        /// </summary>
        [JsonProperty("subreddit_type")]
        public string SubredditType { get; private set; }


        /// <summary>
        /// Gets or sets the vote for the current VotableThing.
        /// </summary>
        [JsonIgnore]
        public VoteType Vote
        {
            get
            {
                switch (this.Liked)
                {
                    case true: return VoteType.Upvote;
                    case false: return VoteType.Downvote;
                    default: return VoteType.None;
                }
            }
            private set
            {
                Task.Run(async () => { await SetVoteAsync(value).ConfigureAwait(false); });
            }
        }
        /// <summary>
        /// Upvotes something
        /// </summary>
        public Task UpvoteAsync()
        {
            return this.SetVoteAsync(VoteType.Upvote);
        }

        /// <summary>
        /// Downvote this item.
        /// </summary>
        public Task DownvoteAsync()
        {
            return this.SetVoteAsync(VoteType.Downvote);
        }

        /// <summary>
        /// Vote on this item.
        /// </summary>
        /// <param name="type"></param>
        public async Task SetVoteAsync(VoteType type)
        {
            if (this.Vote == type) return;

            var data = await WebAgent.Post(VoteUrl, new
            {
                dir = (int)type,
                id = FullName
            }).ConfigureAwait(false);

            if (Liked == true) Upvotes--;
            if (Liked == false) Downvotes--;

            switch (type)
            {
                case VoteType.Upvote: Liked = true; Upvotes++; return;
                case VoteType.None: Liked = null; return;
                case VoteType.Downvote: Liked = false; Downvotes++; return;
            }
        }

        /// <summary>
        /// Save this item.
        /// </summary>
        public async Task SaveAsync()
        {
            await WebAgent.Post(SaveUrl, new
            {
                id = FullName
            }).ConfigureAwait(false);
            Saved = true;
        }

        /// <summary>
        /// Unsave this item.
        /// </summary>
        public async Task UnsaveAsync()
        {
            await WebAgent.Post(UnsaveUrl, new
            {
                id = FullName
            }).ConfigureAwait(false);
            Saved = false;
        }

        /// <summary>
        /// Clear you vote on this item.
        /// </summary>
        public async Task ClearVote()
        {
            await WebAgent.Post(VoteUrl, new
            {
                dir = 0,
                id = FullName
            }).ConfigureAwait(false);
        }
    }
}
