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
        private readonly AmazonDynamoDBClient _dynamoClient;

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
                TableName = "Tweets",
                IndexName = "Hashtag-Index",
                KeyConditionExpression = "Hashtag = :hashtag",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
        {
            { ":hashtag", new AttributeValue { S = hashtag } }
        }
            };

            var response = await _dynamoClient.QueryAsync(request);
            return response.Items.Select(item =>
            {
                return new Tweet
                {
                    Id = int.Parse(item["Id"].N),
                    UserId = int.Parse(item["UserId"].N),
                    Text = item["Text"].S,
                    CreatedAt = DateTime.Parse(item["CreatedAt"].S)
                };
            }).ToList();
        }
    }
}
