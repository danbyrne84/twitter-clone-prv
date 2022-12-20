using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterClone.Data;
using TwitterClone.Models;

namespace TwitterClone.Services
{
    public class TweetService
    {
        private readonly ITweetDataAccess _tweetDataAccess;
        private readonly IMemoryCacheService _memoryCacheService;

        public TweetService(ITweetDataAccess tweetDataAccess, IMemoryCacheService memoryCacheService)
        {
            _tweetDataAccess = tweetDataAccess;
            this._memoryCacheService = memoryCacheService;
        }

        public async Task<List<Tweet>> GetTimelineAsync(int maxTweets)
        {
            // retrieve and return the current user's timeline, with a maximum number of tweets specified by the maxTweets parameter
            return await _tweetDataAccess.GetTimelineAsync(maxTweets);
        }

        public async Task<long> CreateTweetAsync(long userId, string text)
        {
            // create a new tweet with the specified text and return the ID of the newly created tweet
            var newId = await _memoryCacheService.IncrementTweetCountAsync();

            return await _tweetDataAccess.CreateTweetAsync(newId, userId, text);
        }

        public async Task<Tweet> GetTweetAsync(long tweetId)
        {
            // retrieve and return a tweet with the specified ID
            return await _tweetDataAccess.GetTweetAsync(tweetId);
        }
    }
}
