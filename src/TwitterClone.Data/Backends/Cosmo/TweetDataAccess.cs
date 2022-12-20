using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using TwitterClone.Models;

namespace TwitterClone.Data.Backends.CosmosDb
{
    public class TweetDataAccess : ITweetDataAccess
    {
        private readonly CosmosClient _client;
        private readonly Container _tweetsContainer;

        public TweetDataAccess(CosmosClient client)
        {
            _client = client;
            _tweetsContainer = _client.GetContainer("tweeter", "tweets");
        }

        public async Task<List<Tweet>> GetTimelineAsync(int maxTweets)
        {
            if (maxTweets == 0) maxTweets = 5;

            // Query the tweets collection for the most recent tweets, sorted in reverse chronological order
            var query = new QueryDefinition("SELECT * FROM tweets t ORDER BY t.createdAt DESC LIMIT @maxTweets")
                .WithParameter("@maxTweets", maxTweets);

            var queryText = query.QueryText;

            var iterator = _tweetsContainer.GetItemQueryIterator<Tweet>(query);

            var tweets = new List<Tweet>();

            // Iterate over the results of the query and add them to the list of tweets
            while (iterator.HasMoreResults)
            {
                var results = await iterator.ReadNextAsync();
                tweets.AddRange(results);
            }

            return tweets;
        }

        public async Task<long> CreateTweetAsync(long userId, long tweetId, string text)
        {
            // Create a new tweet document with a unique ID and the current timestamp
            var tweet = new Tweet
            {
                Id = tweetId,
                UserId = userId,
                Text = text,
                CreatedAt = DateTime.UtcNow
            };

            // Insert the tweet document into the tweets collection
            var result = await _tweetsContainer.CreateItemAsync(tweet);

            return result.Resource.Id;
        }

        public async Task<Tweet> GetTweetAsync(long tweetId)
        {
            // Retrieve the tweet document from the tweets collection using its ID as the partition key
            var result = await _tweetsContainer.ReadItemAsync<Tweet>(tweetId.ToString(), new PartitionKey(tweetId.ToString()));

            return result.Resource;
        }
    }
}
