using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditSharp.Modmail;


namespace RedditSharp.Modmail
{
    public static class ModmailExtensions
    {
        public static ModmailListing GetModmail(
                this Reddit reddit,
                ModmailListing.ModmailSort sort = ModmailListing.ModmailSort.Recent,
                ModmailListing.ModmailState state = ModmailListing.ModmailState.New,
                int max = -1) =>

            new ModmailListing(
                reddit.WebAgent,
                $"/api/mod/conversations?sort={sort.ToString().ToLower()}&state={state.ToString().ToLower()}",
                max
            );


        public static async Task<Conversation> GetConversation(this Reddit reddit, string id) =>
            new Conversation(
                    reddit.WebAgent,
                    await reddit.WebAgent.Get($"{Conversation.ENDPOINT}/{id}")
            );
    }

}
