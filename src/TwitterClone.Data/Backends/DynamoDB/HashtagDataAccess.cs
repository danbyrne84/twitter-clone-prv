using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Data.Backends.DynamoDb
{
    public class HashtagDataAccess : IHashtagDataAccess
    {

        private AmazonDynamoDBClient _dynamoClient;

        public HashtagDataAccess()
        {

        }

        public HashtagDataAccess(AmazonDynamoDBClient dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<List<Tweet>> GetTweetsForHashtagAsync(string hashtag)
        {
            var request = new QueryRequest
            {
                TableName = "Hashtags",
                IndexName = "Tag-Index",
                KeyConditionExpression = "Tag = :hashtag",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":hashtag", new AttributeValue { S = hashtag } }
                }
            };

            var response = await _dynamoClient.QueryAsync(request);
            var tweetLists = response.Items.Select(item =>
            {
                var tweetIds = item["TweetIds"].L.Select(x => x.N).ToList().ConvertAll(int.Parse);
                var tweets = new List<Tweet>();
                foreach (var tweetId in tweetIds)
                {
                    tweets.Add(GetTweetByIdAsync(tweetId).Result);
                }
                return tweets;
            });
            return tweetLists.SelectMany(x => x).ToList();
        }

        public async Task<Tweet> GetTweetByIdAsync(int tweetId)
        {
            var request = new GetItemRequest
            {
                TableName = "Tweets",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { N = tweetId.ToString() } }
                }
            };

            var response = await _dynamoClient.GetItemAsync(request);
            return new Tweet
            {
                Id = int.Parse(response.Item["Id"].N),
                UserId = int.Parse(response.Item["UserId"].N),
                Text = response.Item["Text"].S,
                CreatedAt = DateTime.Parse(response.Item["CreatedAt"].S)
            };
        }
    }
}
