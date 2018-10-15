using Newtonsoft.Json;

namespace RedditSharp.Modmail
{
    public abstract class ModmailObject : RedditObject
    {
        [JsonProperty]
        public string Id { get; set; }

        /// <inheritdoc />
        public ModmailObject(IWebAgent agent) : base(agent)
        {
        }
    }
}
