using System;
using System.Collections.Generic;
using LinqToTwitter;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            var tweetList = ConsoleHelper.GetTwitterFeeds();

            UserDetails(tweetList);
            TweetDetails(tweetList);
            NoOfTweetsPerCategory(tweetList);
            WriteTweetToFile(tweetList);

            Console.ReadLine();
        }

        private static void NoOfTweetsPerCategory(List<Status> tweetList)
        {
            //# of tweets 
            int hashTagTweetCount = 0;
            int mediaTweetCount = 0;
            int urlTweetCount = 0;
            int userTweetCount = 0;
            List<TextCounter> hashTagTextList = new List<TextCounter>();

            foreach (var item in tweetList)
            {
                if (item.Entities != null)
                {
                    if (item.Entities.HashTagEntities.Count > 0)
                    {
                        hashTagTweetCount = ConsoleHelper.HashTagTweetCount(item, hashTagTextList, hashTagTweetCount);
                    }

                    // Similar process can be done for Media, URL and User
                    if (item.Entities.MediaEntities.Count > 0)
                        mediaTweetCount++;
                    if (item.Entities.UrlEntities.Count > 0)
                        urlTweetCount++;
                    if (item.Entities.UserMentionEntities.Count > 0)
                        userTweetCount++;
                }
            }

            string topHasTag = ConsoleHelper.GetTopHasTag(hashTagTextList);

            Console.WriteLine(" % tweets has #Tag:  " + ((100 * hashTagTweetCount) / tweetList.Count));
            Console.WriteLine("          Top #Tag:  " + topHasTag + "\n");
            Console.WriteLine(" % tweets has Media:  " + ((100 * mediaTweetCount) / tweetList.Count));
            Console.WriteLine(" % tweets has   URL:  " + ((100 * urlTweetCount) / tweetList.Count));
            Console.WriteLine(" % tweets has  User:  " + ((100 * userTweetCount) / tweetList.Count));
        }

        private static void UserDetails(List<Status> tweetList)
        {
            //# of Followers I have
            Console.WriteLine("# of followers I have: " + tweetList[0].User.FollowersCount);
        }

        private static void TweetDetails(List<Status> tweetList)
        {
            //# of Tweets
            Console.WriteLine("  Total no. Of Tweets: " + tweetList.Count);

            //# of Tweets per hour
            DateTime lastTweet = tweetList[0].CreatedAt;
            DateTime firstTweet = tweetList[tweetList.Count - 1].CreatedAt;
            TimeSpan differenceTime = lastTweet - firstTweet;
            Console.WriteLine("# of tweets per hour:  " + (tweetList.Count / Math.Round(differenceTime.TotalHours)) + "\n");
        }

        private static void WriteTweetToFile(List<Status> tweetList)
        {
            //# Tweet Data
            var file = new System.IO.StreamWriter("C:\\Users\\patel\\Desktop\\TweetsList.txt", true);
            foreach (var item in tweetList)
            {
                file.WriteLine(item.CreatedAt + " --- : " + item.Text);
            }

            file.Close();
        }
    }
}

