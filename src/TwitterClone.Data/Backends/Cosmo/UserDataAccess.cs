using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterClone.Models;
using User = TwitterClone.Models.User;

namespace TwitterClone.Data.Backends.CosmosDb
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseName = "tweeter";
        private readonly string _containerName = "tweets";

        public UserDataAccess(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public async Task<List<Tweet>> GetUserTweetsAsync(int userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            var response = await container.GetItemQueryIterator<Tweet>(query).ReadNextAsync();
            return response.ToList();
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Id = @userId")
                .WithParameter("@userId", userId);
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            var response = await container.GetItemQueryIterator<User>(query).ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<List<User>> GetFollowersAsync(int userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            var response = await container.GetItemQueryIterator<Follower>(query).ReadNextAsync();
            var followerIds = response.Select(f => f.FollowerId).ToList();

            var users = await GetUsersByIdAsync(followerIds);
            return users;
        }
        private async Task<List<User>> GetUsersByIdAsync(List<int> userIds)
        {
            var users = new List<User>();
            foreach (var userId in userIds)
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.Id = @userId")
                    .WithParameter("@userId", userId);
                var container = _cosmosClient.GetContainer(_databaseName, _containerName);
                var response = await container.GetItemQueryIterator<User>(query).ReadNextAsync();
                users.AddRange(response);
            }
            return users;
        }


        public async Task<List<User>> GetFollowingsAsync(int userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            var response = await container.GetItemQueryIterator<Following>(query).ReadNextAsync();
            var followingIds = response.Select(f => f.FollowingId).ToList();
            var users = await GetUsersByIdAsync(followingIds);
            return users;
        }

        public async Task<List<string>> GetHashtagsAsync(int tweetId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE ARRAY_CONTAINS(c.TweetIds, @tweetId)")
                .WithParameter("@tweetId", tweetId);
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            var response = await container.GetItemQueryIterator<Hashtag>(query).ReadNextAsync();
            return response.Select(h => h.Tag).ToList();
        }

        public async Task AddTweetAsync(Tweet tweet)
        {
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            await container.CreateItemAsync(tweet);
        }

        public async Task AddUserAsync(User user)
        {
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            await container.CreateItemAsync(user);
        }

        public async Task FollowAsync(int userId, int followerId)
        {
            var follower = new Follower
            {
                UserId = userId,
                FollowerId = followerId
            };
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            await container.CreateItemAsync(follower);
        }

        public async Task UnfollowAsync(int userId, int followerId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId AND c.FollowerId = @followerId")
                .WithParameter("@userId", userId)
                .WithParameter("@followerId", followerId);
            var container = _cosmosClient.GetContainer(_databaseName, _containerName);
            var response = await container.GetItemQueryIterator<Follower>(query).ReadNextAsync();
            var follower = response.FirstOrDefault();
            if (follower != null)
            {
                await container.DeleteItemAsync<Follower>(follower.Id, new PartitionKey(follower.UserId));
            }
        }
    }
}