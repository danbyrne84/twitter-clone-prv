using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Data.Backends.DynamoDb
{

    public class TweetDataAccess : ITweetDataAccess
    {
        private readonly AmazonDynamoDBClient _dynamoClient;

        public TweetDataAccess(AmazonDynamoDBClient dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<List<Tweet>> GetTimelineAsync(int maxTweets)
        {
            // construct the query to retrieve the current user's timeline from the DynamoDB table
            var queryRequest = new QueryRequest
            {
                TableName = "Tweets",
                KeyConditionExpression = "UserId = :userId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":userId", new AttributeValue { N = "123" } } // replace with the ID of the current user
                },
                    Limit = maxTweets,
                    ScanIndexForward = false, // retrieve tweets in reverse chronological order
                    IndexName = "CreatedAt-Index"
            };

            // execute the query
            var queryResponse = await _dynamoClient.QueryAsync(queryRequest);

            // map the query results to a list of Tweet objects
            return queryResponse.Items.Select(item => new Tweet
            {
                Id = int.Parse(item["Id"].N),
                UserId = int.Parse(item["UserId"].N),
                Text = item["Text"].S,
                CreatedAt = DateTime.Parse(item["CreatedAt"].S)
            }).ToList();
        }

        public async Task<long> CreateTweetAsync(long userId, long tweetId, string text)
        {

            // construct the PutItem request to create the new tweet in the DynamoDB table
            var putItemRequest = new PutItemRequest
            {
                TableName = "Tweets",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { N = tweetId.ToString() } },
                    { "UserId", new AttributeValue { N = userId.ToString() } }, // replace with the ID of the current user
                    { "Text", new AttributeValue { S = text } },
                    { "CreatedAt", new AttributeValue { S = DateTime.UtcNow.ToString("O") } }
                }
            };

            // execute the PutItem request
            await _dynamoClient.PutItemAsync(putItemRequest);

            // return the ID of the newly created tweet
            return tweetId;
        }

        public async Task<Tweet> GetTweetAsync(long tweetId)
        {
            // construct the GetItem request to retrieve the tweet from the DynamoDB table
            var getItemRequest = new GetItemRequest
            {
                TableName = "Tweets",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { N = tweetId.ToString() } }
                }
            };

            // execute the GetItem request
            var getItemResponse = await _dynamoClient.GetItemAsync(getItemRequest);

            // map the item retrieved to a Tweet object
            return new Tweet
            {
                Id = int.Parse(getItemResponse.Item["Id"].N),
                UserId = int.Parse(getItemResponse.Item["UserId"].N),
                Text = getItemResponse.Item["Text"].S,
                CreatedAt = DateTime.Parse(getItemResponse.Item["CreatedAt"].S)
            };
        }
    }
}