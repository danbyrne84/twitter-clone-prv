using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using TwitterClone.Models;

namespace TwitterClone.Data.Backends.CosmosDb
{
    public class HashtagDataAccess : IHashtagDataAccess
    {
        private readonly CosmosClient _client;
        private readonly Container _hashtagsContainer;
        private readonly Container _tweetsContainer;

        public HashtagDataAccess(CosmosClient client)
        {
            _client = client;
            _hashtagsContainer = _client.GetContainer("tweeter", "hashtags");
            _tweetsContainer = _client.GetContainer("tweeter", "tweets");
        }

        public async Task<List<Tweet>> GetTweetsForHashtagAsync(string hashtag)
        {
            // Query the hashtags collection for the specified hashtag
            var query = new QueryDefinition("SELECT * FROM hashtags h WHERE h.tag = @tag")
                .WithParameter("@tag", hashtag);

            var iterator = _hashtagsContainer.GetItemQueryIterator<Hashtag>(query);

            var tweets = new List<Tweet>();

            // Iterate over the results of the query
            while (iterator.HasMoreResults)
            {
                var results = await iterator.ReadNextAsync();
                foreach (var result in results)
                {
                    // For each tweet ID in the hashtag document, retrieve the tweet from the tweets collection
                    foreach (var tweetId in result.TweetIds)
                    {
                        var tweet = await _tweetsContainer.ReadItemAsync<Tweet>(tweetId.ToString(), new PartitionKey(tweetId));
                        tweets.Add(tweet);
                    }
                }
            }

            return tweets;
        }
    }
}
