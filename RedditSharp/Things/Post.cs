using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RedditSharp.Things
{
    /// <summary>
    /// A post.
    /// </summary>
    public class Post : VotableThing
    {
        private const string CommentUrl = "/api/comment";
        private const string GetCommentsUrl = "/comments/{0}.json";
        private const string EditUserTextUrl = "/api/editusertext";
        private const string HideUrl = "/api/hide";
        private const string UnhideUrl = "/api/unhide";
        private const string SetFlairUrl = "/r/{0}/api/flair";
        private const string MarkNSFWUrl = "/api/marknsfw";
        private const string UnmarkNSFWUrl = "/api/unmarknsfw";
        private const string ContestModeUrl = "/api/set_contest_mode";
        private const string StickyModeUrl = "/api/set_subreddit_sticky";
        /// <inheritdoc />
        public Post(IWebAgent agent, JToken json) : base(agent, json)
        {
        }

        /// <summary>
        /// Returns true if post is marked as spoiler
        /// </summary>
        [JsonProperty("spoiler")]
        public bool IsSpoiler { get; private set; }
        
        /// <summary>
        /// Returns true if this post is hidden
        /// </summary>
        [JsonProperty("hidden")]
        public bool IsHidden { get; private set; }
        
        /// <summary>
        /// Domain of this post.
        /// </summary>
        [JsonProperty("domain")]
        public string Domain { get; private set; }

        /// <summary>
        /// Returns true if this is a self post.
        /// </summary>
        [JsonProperty("is_self")]
        public bool IsSelfPost { get; private set; }

        /// <summary>
        /// Link flair background color in hex triplet.
        /// </summary>
        [JsonProperty("link_flair_background_color")]
        public string LinkFlairBackgroundColor { get; private set; }

        /// <summary>
        /// Link flair template.
        /// </summary>
        [JsonProperty("link_flair_template_id")]
        public string LinkFlairTemplateId { get; private set; }

        /// <summary>
        /// Link flair text color.
        /// </summary>
        [JsonProperty("link_flair_text_color")]
        public string LinkFlairTextColor { get; private set; }

        /// <summary>
        /// Link flair type.
        /// </summary>
        [JsonProperty("link_flair_type")]
        public string LinkFlairType { get; private set; }

        /// <summary>
        /// Css class of the link flair.
        /// </summary>
        [JsonProperty("link_flair_css_class")]
        public string LinkFlairCssClass { get; private set; }

        /// <summary>
        /// Text of the link flair.
        /// </summary>
        [JsonProperty("link_flair_text")]
        public string LinkFlairText { get; private set; }

        /// <summary>
        /// Number of comments on this post.
        /// </summary>
        [JsonProperty("num_comments")]
        public int CommentCount { get; private set; }

        /// <summary>
        /// Returns true if this post is marked not safe for work.
        /// </summary>
        [JsonProperty("over_18")]
        public bool NSFW { get; private set; }

        /// <summary>
        /// Post self text markdown.
        /// </summary>
        [JsonProperty("selftext")]
        public string SelfText { get; private set; }

        /// <summary>
        /// Post self text html.
        /// </summary>
        [JsonProperty("selftext_html")]
        public string SelfTextHtml { get; private set; }

        /// <summary>
        /// Uri to the thumbnail image of this post.
        /// </summary>
        [JsonProperty("thumbnail")]
        [JsonConverter(typeof(UrlParser))]
        public Uri Thumbnail { get; private set; }

        /// <summary>
        /// Post title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        /// Parent subreddit name.
        /// </summary>
        [JsonProperty("subreddit")]
        public string SubredditName { get; private set; }

        /// <summary>
        /// Parent subreddit name.
        /// </summary>
        [JsonProperty("view_count")]
        public int? ViewCount { get; private set; }

        /// <summary>
        /// Clicked
        /// </summary>
        [JsonProperty("clicked")]
        public bool IsClicked { get; private set; }

        /// <summary>
        /// True if this post is set to contest mode.
        /// </summary>
        [JsonProperty("contest_mode")]
        public bool IsContestMode { get; private set; }

        /// <summary>
        /// True if this post is locked.
        /// </summary>
        [JsonProperty("locked")]
        public bool IsLocked { get; private set; }

        /// <summary>
        /// Prefix for fullname. Includes trailing underscore
        /// </summary>
        public static string KindPrefix => "t3_";

        /// <summary>
        /// Gildings.
        /// </summary>
        [JsonProperty("gildings")]
        public IDictionary<string,int> Gildings { get; private set; }

        /// <summary>
        /// True if reports are ignored.  Null if not a moderator this parent subreddit.
        /// </summary>
        [JsonProperty("is_crosspostable")]
        public bool IsCrosspostable { get; set; }

        /// <summary>
        /// True if this post is marked for meta discussion.
        /// </summary>
        [JsonProperty("ignore_reports")]
        public bool IsMeta { get; set; }

        /// <summary>
        /// True if this post is original content.
        /// </summary>
        [JsonProperty("is_original_content")]
        public bool IsOriginalContent { get; set; }

        /// <summary>
        /// True if this post is from a reddit media domain.
        /// </summary>
        [JsonProperty("is_reddit_media_domain")]
        public bool IsRedditMediaDomain { get; set; }

        /// <summary>
        /// True if this post is indexable by bots.
        /// </summary>
        [JsonProperty("is_robot_indexable")]
        public bool IsRobotIndexable { get; set; }

        /// <summary>
        /// True if this post is / contains video.
        /// </summary>
        [JsonProperty("is_video")]
        public bool IsVideo { get; set; }

        /// <summary>
        /// Post uri.
        /// </summary>
        [JsonProperty("url")]
        [JsonConverter(typeof(UrlParser))]
        public Uri Url { get; private set; }

        /// <summary>
        /// True if score is hidden
        /// </summary>
        [JsonProperty("hide_score")]
        public bool HideScore { get; private set; }

        /// <summary>
        /// Number of crossposts
        /// </summary>
        [JsonProperty("num_crossposts")]
        public int? CrosspostCount { get; private set; }

        /// <summary>
        /// Whitelist status.
        /// </summary>
        [JsonProperty("parent_whitelist_status")]
        public string ParentWhitelistStatus { get; private set; }

        /// <summary>
        /// Post hint.
        /// </summary>
        [JsonProperty("post_hint")]
        public string PostHint { get; private set; }

        /// <summary>
        /// Post hint.
        /// </summary>
        [JsonProperty("previous_visits")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public ICollection<DateTime> PreviousVisits { get; private set; }

        /// <summary>
        /// True if this post (parent subreddit) is quarantined.
        /// </summary>
        [JsonProperty("is_quarantine")]
        public bool IsQuarantine { get; private set; }

        /// <summary>
        /// Number of subscribers to the parent subreddit.
        /// </summary>
        [JsonProperty("subreddit_subscribers")]
        public int SubredditSubscriberCount { get; private set; }

        /// <summary>
        /// Suggested sort of the post.
        /// </summary>
        [JsonProperty("suggested_sort")]
        public Sort? SuggestedSort { get; private set; }

        /// <summary>
        /// Height of thumbnail in pixels.
        /// </summary>
        [JsonProperty("thumbnail_height")]
        public int? ThumbnailHeight { get; private set; }

        /// <summary>
        /// Width of thumbnail in pixels.
        /// </summary>
        [JsonProperty("thumbnail_width")]
        public int? ThumbnailWidth { get; private set; }

        /// <summary>
        /// Upvote ratio of the post.
        /// </summary>
        [JsonProperty("upvote_ratio")]
        public decimal UpvoteRatio { get; private set; }

        /// <summary>
        /// Visited.
        /// </summary>
        [JsonProperty("visited")]
        public bool Visited { get; private set; }

        /// <summary>
        /// Whitelist status.
        /// </summary>
        [JsonProperty("whitelist_status")]
        public string WhitelistStatus { get; private set; }


        /// <summary>
        /// Returns the parent <see cref="Subreddit"/> for this post
        /// </summary>
        /// <returns></returns>
        public Task<Subreddit> GetSubredditAsync()
        {
            return Subreddit.GetByNameAsync(WebAgent, SubredditName);
        }

        /// <summary>
        /// Comment on this post.
        /// </summary>
        /// <param name="message">Markdown text.</param>
        /// <returns></returns>
        public async Task<Comment> CommentAsync(string message)
        {
            var json = await WebAgent.Post(CommentUrl, new
            {
                text = message,
                thing_id = FullName,
                api_type = "json"
            }).ConfigureAwait(false);
            if (json["errors"].Any())
            {
                if (json["errors"][0].Any(x => x.ToString() == "RATELIMIT" || x.ToString() == "ratelimit"))
                {
                    var timeToReset = TimeSpan.FromMinutes(Convert.ToDouble(Regex.Match(json["errors"][0].ElementAt(1).ToString(), @"\d+").Value));
                    throw new RateLimitException(timeToReset);
                }
                else
                {
                    throw new Exception(json["errors"][0][0].ToString());
                }
            }

            return new Comment(WebAgent, json["data"]["things"][0], this);
        }

        private async Task<JToken> SimpleActionToggleAsync(string endpoint, bool value, bool requiresModAction = false)
        {
            return await WebAgent.Post(endpoint, new
            {
                id = FullName,
                state = value
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Hide this post.
        /// </summary>
        public Task HideAsync() => SimpleActionAsync(HideUrl);

        /// <summary>
        /// Unhide this post.
        /// </summary>
        public Task UnhideAsync() => SimpleActionAsync(UnhideUrl);

        /// <summary>
        /// Mark this post not safe for work.
        /// </summary>
        public Task MarkNSFWAsync() => SimpleActionAsync(MarkNSFWUrl);

        /// <summary>
        /// Mark this post as safe for work.
        /// </summary>
        public Task UnmarkNSFWAsync() => SimpleActionAsync(UnmarkNSFWUrl);

        /// <summary>
        /// Set contest mode state.  Logged in user must be a moderator of parent subreddit.
        /// </summary>
        /// <param name="state"></param>
        public Task ContestModeAsync(bool state) => SimpleActionAsync(ContestModeUrl);

        /// <summary>
        /// Set sticky state.  Logged in user must be a moderator of parent subreddit.
        /// </summary>
        /// <param name="state"></param>
        public Task StickyModeAsync(bool state) => SimpleActionToggleAsync(StickyModeUrl, state, true);

        /// <summary>
        /// Replaces the text in this post with the input text.
        /// </summary>
        /// <param name="newText">The text to replace the post's contents</param>
        public async Task EditTextAsync(string newText)
        {
            if (!IsSelfPost)
                throw new Exception("Submission to edit is not a self-post.");

            var json = await WebAgent.Post(EditUserTextUrl, new
            {
                api_type = "json",
                text = newText,
                thing_id = FullName
            }).ConfigureAwait(false);
            if (!json["errors"].Any())
                SelfText = newText;
            else
                throw new Exception("Error editing text.");
        }

        /// <summary>
        /// Updates data retrieved for this post.
        /// </summary>
        public async Task UpdateAsync() => Helpers.PopulateObject(GetJsonData(await Helpers.GetTokenAsync(WebAgent, Url)), this);

        /// <summary>
        /// Sets your flair
        /// </summary>
        /// <param name="flairText">Text to set your flair</param>
        /// <param name="flairClass">class of the flair</param>
        public async Task SetFlairAsync(string flairText, string flairClass)
        {
            //TODO Unit test
            await Post.SetFlairAsync(this.WebAgent, this.SubredditName, this.FullName, flairText, flairClass).ConfigureAwait(false);
            LinkFlairText = flairText;
        }

        /// <summary>
        /// Get a <see cref="List{T}"/> of comments.
        /// </summary>
        /// <param name="limit">Maximum number of comments to return</param>
        /// <returns></returns>
        public async Task<List<Comment>> GetCommentsAsync(int limit = 0, CommentSort sort = CommentSort.Best)
        {
            var url = string.Format(GetCommentsUrl, Id);

            //Only 'best' comment sorting isn't named the same
            if (sort == CommentSort.Best)
            {
                url = $"{url}?sort=confidence";
            }
            else
            {
                url = $"{url}?sort={sort.ToString().ToLower()}";
            }

            if (limit > 0)
            {
                url = $"{url}&limit={limit}";
            }

            var json = await WebAgent.Get(url).ConfigureAwait(false);
            var postJson = json.Last()["data"]["children"];

            var comments = new List<Comment>();
            foreach (var comment in postJson)
            {
                Comment newComment = new Comment(WebAgent, comment, this);
                if (newComment.Kind != "more")
                    comments.Add(newComment);
            }

            return comments;
        }

        /// <summary>
        /// Returns a <see cref="List{T}"/> of <see cref="Thing"/> that contains <see cref="Comment"/> and <see cref="More"/>
        /// </summary>
        /// <param name="limit">Maximum number of comments to return. Returned list may be larger than this number though due to <see cref="More"/></param>
        /// <returns></returns>
        public async Task<List<Thing>> GetCommentsWithMoresAsync(int limit = 0, CommentSort sort = CommentSort.Best)
        {
            var url = string.Format(GetCommentsUrl, Id);

            //Only 'best' comment sorting isn't named the same
            if (sort == CommentSort.Best)
            {
                url = $"{url}?sort=confidence";
            }
            else
            {
                url = $"{url}?sort={sort.ToString().ToLower()}";
            }

            if (limit > 0)
            {
                url = $"{url}&limit={limit}";
            }

            var json = await WebAgent.Get(url).ConfigureAwait(false);
            var postJson = json.Last()["data"]["children"];

            var things = new List<Thing>();
            foreach (var comment in postJson)
            {
                Comment newComment = new Comment(WebAgent, comment, this);
                if (newComment.Kind != "more")
                {
                    things.Add(newComment);
                }
                else
                {
                    things.Add(new More(WebAgent, comment));
                }
            }

            return things;

        }
        /// <summary>
        /// Returns an <see cref="IAsyncEnumerable{T}"/> of <see cref="Comment"/> containing all comments in a post.
        /// This will cause multiple web requests on larger comment sections.
        /// </summary>
        /// <param name="limitPerRequest">Maximum number of comments to retrieve at a time. 0 for Reddit maximum</param>
        /// <returns></returns>
        public IAsyncEnumerable<Comment> EnumerateCommentTreeAsync(int limitPerRequest = 0)
        {
            return new CommentsEnumarable(WebAgent, this, limitPerRequest);
        }
#region Static Operations
        /// <summary>
        /// Sets flair of given post by <paramref name="fullname"/>
        /// </summary>
        /// <param name="agent"><see cref="IWebAgent"/> used to send post</param>
        /// <param name="fullname">FullName of thing to act on. eg. t1_66666</param>
        /// <param name="subreddit">Subreddit name of post</param>
        /// <param name="flairText">Text of flair to set</param>
        /// <param name="flairClass">Css class name of flair to set</param>
        /// <returns></returns>
        public static Task SetFlairAsync(IWebAgent agent, string subreddit, string fullname, string flairText, string flairClass)
        {
            //TODO unit test
            return agent.Post(string.Format(SetFlairUrl, subreddit), new
            {
                api_type = "json",
                css_class = flairClass,
                link = fullname,
                text = flairText
            });
        }
        #endregion
    }

}

