using System;
using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Linq.Dynamic;

namespace $rootnamespace$.Application.Social
{
    public class Twitter
    {
        public static IEnumerable<Status> GetLatestTweet(List<string> hashTags, int numberOfTweets = 1)
        {
            List<Status> latestTweets = new List<Status>();

            if (hashTags != null && hashTags.Count > 0)
            {
                String strConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"];
                String strConsumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"];
                String strOAuthToken = ConfigurationManager.AppSettings["twitterAccessToken"];
                String strOAuthTokenSecret = ConfigurationManager.AppSettings["twitterAccessTokenSecret"];

                if (!String.IsNullOrEmpty(strConsumerKey) || !String.IsNullOrEmpty(strConsumerSecret) || !String.IsNullOrEmpty(strOAuthToken) || !String.IsNullOrEmpty(strOAuthTokenSecret))
                {
                    var auth = new SingleUserAuthorizer
                    {
                        CredentialStore = new SessionStateCredentialStore
                        {
                            ConsumerKey = strConsumerKey,
                            ConsumerSecret = strConsumerSecret,
                            OAuthToken = strOAuthToken,
                            OAuthTokenSecret = strOAuthTokenSecret
                        }
                    };

                    try
                    {
                        var twitterCtx = new TwitterContext(auth);

                        var tweets = (from tweet in twitterCtx.Status where tweet.Type == StatusType.User select tweet);
                        List<Status> lstRawTweets = new List<Status>();

                        if (tweets != null && tweets.ToList().Count() > 0)
                        {
                            lstRawTweets = tweets.ToList();
                            lstRawTweets = lstRawTweets.OrderByDescending(x => x.CreatedAt).ToList();
                        }

                        List<Status> tweetList = new List<Status>();

                        foreach (string tag in hashTags)
                        {
                            var tagTweets = lstRawTweets.Where(x => x.Text.IndexOf(tag) >= 0).ToList();

                            if (tagTweets != null && tagTweets.Count > 0)
                            {
                                tweetList.AddRange(tagTweets);
                            }
                        }

                        if (tweetList != null && tweetList.Count() > 0)
                        {
                            tweetList = tweetList.OrderByDescending(x => x.CreatedAt).ToList();
                            var lstTweets = tweetList.Take(numberOfTweets);

                            foreach (var twt in lstTweets)
                            {
                                twt.Text = ConvertUrlsToLinks(twt.Text);
                                latestTweets.Add(twt);
                            }
                        }
                    }
                    catch (TwitterQueryException tqEx)
                    {
                        //Do Nothing - Not authorised
                    }
                }
            }

            if (latestTweets != null && latestTweets.Count > 0)
            {
                latestTweets = latestTweets.OrderByDescending(x => x.CreatedAt).ToList();
            }

            return latestTweets;
        }

        public static string ConvertUrlsToLinks(string rawTweet)
        {
            Regex link = new Regex(@"http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?");
            Regex screenName = new Regex(@"@\w+");
            Regex hashTag = new Regex(@"#\w+");

            string formattedTweet = link.Replace(rawTweet, delegate(Match m)
            {
                string val = m.Value;
                return "<a href='" + val + "'>" + val + "</a>";
            });

            formattedTweet = screenName.Replace(formattedTweet, delegate(Match m)
            {
                string val = m.Value.Trim('@');
                return string.Format("@<a href='http://twitter.com/{0}'>{1}</a>", val, val);
            });

            formattedTweet = hashTag.Replace(formattedTweet, delegate(Match m)
            {
                string val = m.Value;
                return string.Format("<a href='http://twitter.com/search?q=%23{0}'>{1}</a>", val.Replace("#", ""), val);
            });
            return formattedTweet;
        }
    }
}