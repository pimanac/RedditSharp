using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditSharp.Modmail.Converters
{
    public class ModmailUserConverter : ModmailObjectConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new ModmailUser(WebAgent);
            
            serializer.Populate(reader, result);

            var json = JObject.Load(reader);

            // get the recent comments collection
            result._recentComments.Urls = json["recentComments"]
                .Children()
                .Select(c => c["permalink"].Value<string>())
                .ToArray();

            result._recentPosts.Urls = json["recentPosts"]
                .Children()
                .Select(p => p["permalink"].Value<string>())
                .ToArray();

            result._recentConvos.Urls = json["recentConvos"]
                .Children()
                .Select(p => p["permalink"].Value<string>())
                .ToArray();

            return result;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ModmailUser);
        }

        /// <inheritdoc />
        public ModmailUserConverter(IWebAgent agent) : base(agent)
        {
        }
    }
}
/*
{
      "recentComments" : {
         "t1_e7nseky" : {
            "comment" : "I\u0027m ok with that",
            "date" : "2018-10-12T21:40:21.027977+00:00",
            "permalink" : "https://www.reddit.com/r/politics/comments/9nodq6/trumps_basepandering_may_be_hurting_republicans/e7nseky
            /",
            "title" : "Trump’s Base-Pandering May Be Hurting Republicans in Swing Districts"
         },
         "t1_e7nflyr" : {
            "comment" : "Plot twist:\n\nSteve Bannon meets with white nationalists everywhere he goes",
            "date" : "2018-10-12T18:31:42.688313+00:00",
            "permalink" : "https://www.reddit.com/r/politics/comments/9nmk53/steve_bannon_met_a_white_nationalist_facebook/e7nflyr/",
            "title" : "Steve Bannon Met A White Nationalist Facebook Personality During London Trip"
         },
         "t1_e7nril2" : {
            "comment" : "I\u0027ve been saying this for years. There\u0027s nothing inherently wrong with a conservative mindset, in
             fact we need them as counterpoint to the l...",
            "date" : "2018-10-12T21:26:11.112274+00:00",
            "permalink" : "https://www.reddit.com/r/politics/comments/9nnes8/did_hell_freeze_over_my_republican_dad_is_voting/e7nril
            2/",
            "title" : "Did Hell Freeze Over? My Republican Dad Is Voting for a Democrat"
         }
      },
      "muteStatus" : {
         "isMuted" : false,
         "endDate" : null,
         "reason" : ""
      },
      "name" : "dbcspace",
      "created" : "2012-07-24T17:09:12.212718+00:00",
      "banStatus" : {
         "endDate" : null,
         "reason" : "",
         "isBanned" : false,
         "isPermanent" : false
      },
      "isSuspended" : false,
      "isShadowBanned" : false,
      "recentPosts" : {},
      "recentConvos" : {
         "42egj" : {
            "date" : "2018-09-21T17:11:22.056658+00:00",
            "permalink" : "https://mod.reddit.com/mail/perma/42egj",
            "id" : "42egj",
            "subject" : "You\u0027ve been banned from participating in r/politics"
         },
         "5bz46" : {
            "date" : "2018-10-12T17:49:38.046812+00:00",
            "permalink" : "https://mod.reddit.com/mail/perma/5bz46",
            "id" : "5bz46",
            "subject" : "Content Question"
         },
         "53xbd" : {
            "date" : "2018-09-21T14:52:12.257826+00:00",
            "permalink" : "https://mod.reddit.com/mail/perma/53xbd",
            "id" : "53xbd",
            "subject" : "Ban appeal"
         }
      },
      "id" : "t2_8fqti"
   }
 * */
