using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Data.Backends.DynamoDb
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly AmazonDynamoDBClient _dynamoClient;

        public UserDataAccess(AmazonDynamoDBClient dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<List<Tweet>> GetUserTweetsAsync(int userId)
        {
            var request = new QueryRequest
            {
                TableName = "Tweets",
                IndexName = "UserId-Index",
                KeyConditionExpression = "UserId = :userId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":userId", new AttributeValue { N = userId.ToString() } }
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

        public async Task<List<User>> GetFollowersAsync(int userId)
        {
            var request = new QueryRequest
            {
                TableName = "Followers",
                IndexName = "FollowerId-Index",
                KeyConditionExpression = "UserId = :userId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":userId", new AttributeValue { N = userId.ToString() } }
            }
            };

            var response = await _dynamoClient.QueryAsync(request);
            return response.Items.Select(item =>
            {
                return new User
                {
                    Id = int.Parse(item["FollowerId"].N),
                    Username = item["Username"].S,
                    DisplayName = item["DisplayName"].S,
                    Email = item["Email"].S
                };
            }).ToList();
        }

        public async Task<List<User>> GetFollowingsAsync(int userId)
        {
            var request = new QueryRequest
            {
                TableName = "Followings",
                IndexName = "UserId-Index",
                KeyConditionExpression = "FollowerId = :followerId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":followerId", new AttributeValue { N = userId.ToString() } }
                }
            };

            var response = await _dynamoClient.QueryAsync(request);
            return response.Items.Select(item =>
            {
                return new User
                {
                    Id = int.Parse(item["UserId"].N),
                    Username = item["Username"].S,
                    DisplayName = item["DisplayName"].S,
                    Email = item["Email"].S
                };
            }).ToList();
        }
    }
}