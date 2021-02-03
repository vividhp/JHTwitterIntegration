using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LinqToTwitter;

namespace ConsoleApp1
{
    /// <summary>
    /// This class has been created to as helper class for this task
    /// </summary>
    public static class ConsoleHelper
    {
        //Created a seperate method so if the logic changes for First or the Last item can be changed here
        internal static string GetTopHasTag(List<TextCounter> textList)
        {
            string topHasTagWord = String.Empty;

            if (textList == null || textList.Count < 1)
                return topHasTagWord;
            else
            {
                List<TextCounter> sorted = textList.OrderByDescending(x => x.Frequency).ToList();
                return sorted[0].Text;
            }

        }

        //Same method can have another parameter for Entities, Then same method can give list of media, url or person too
        internal static int HashTagTweetCount(Status item, List<TextCounter> textCounters, int hashTagTweetCount)
        {
            foreach (var eachashTagEntity in item.Entities.HashTagEntities)
            {
                TextCounter foundit = textCounters.Find(x => x.Text == eachashTagEntity.Tag);
                if (foundit == null)
                {
                    textCounters.Add(new TextCounter(eachashTagEntity.Tag, 1));
                }
                else
                {
                    foundit.Frequency++;
                }
            }

            hashTagTweetCount++;
            return hashTagTweetCount;
        }

        /// <summary>
        /// This method is to get all the tweets for iuser account
        /// </summary>
        /// <returns></returns>
        internal static List<Status> GetTwitterFeeds()
        {
            //string screenname = "vividHp";

            var auth = new SingleUserAuthorizer
            {

                CredentialStore = new InMemoryCredentialStore()
                {

                    // All keys are found in App.Config file
                    ConsumerKey = ConfigurationManager.AppSettings["consumerkey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["consumersecret"],
                    OAuthToken = ConfigurationManager.AppSettings["accessToken"],
                    OAuthTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"]
                }

            };
            var twitterCtx = new TwitterContext(auth);
            var ownTweets = new List<Status>();

            ulong maxId = 0;
            bool flag = true;
            var statusResponse = new List<Status>();
            statusResponse = (from tweet in twitterCtx.Status
                              where tweet.Type == StatusType.User
                              //&& tweet.ScreenName == screenname
                              && tweet.Count == 200
                              select tweet).ToList();

            if (statusResponse.Count > 0)
            {
                maxId = ulong.Parse(statusResponse.Last().StatusID.ToString()) - 1;
                ownTweets.AddRange(statusResponse);
            }
            do
            {
                int rateLimitStatus = twitterCtx.RateLimitRemaining;
                if (rateLimitStatus != 0)
                {

                    statusResponse = (from tweet in twitterCtx.Status
                                      where tweet.Type == StatusType.User
                                      //&& tweet.ScreenName == screenname
                                      && tweet.MaxID == maxId
                                      && tweet.Count == 200
                                      select tweet).ToList();

                    if (statusResponse.Count != 0)
                    {
                        maxId = ulong.Parse(statusResponse.Last().StatusID.ToString()) - 1;
                        ownTweets.AddRange(statusResponse);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            } while (flag);

            return ownTweets;
        }
    }
}
