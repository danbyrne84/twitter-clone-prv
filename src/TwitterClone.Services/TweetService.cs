using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Services
{
    public class TweetService
    {
        public async Task<List<Tweet>> GetTimelineAsync(int maxTweets)
        {
            // retrieve and return the current user's timeline, with a maximum number of tweets specified by the maxTweets parameter
            throw new NotImplementedException();
        }

        public async Task<int> CreateTweetAsync(string text)
        {
            // create a new tweet with the specified text and return the ID of the newly created tweet
            throw new NotImplementedException();
        }

        public async Task<Tweet> GetTweetAsync(int tweetId)
        {
            // retrieve and return a tweet with the specified ID
            throw new NotImplementedException();
        }
    }



    public class HashtagService
    {
        public async Task<List<Tweet>> GetTweetsForHashtagAsync(string hashtag)
        {
            // retrieve and return a list of tweets that contain the specified hashtag
            throw new NotImplementedException();
        }
    }

}
