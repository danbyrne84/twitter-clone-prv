using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Services
{
    public class UserService
    {
        public async Task<List<Tweet>> GetUserTweetsAsync(int userId)
        {
            // retrieve and return a list of tweets for the user with the specified ID
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetFollowersAsync(int userId)
        {
            // retrieve and return a list of users who are following the user with the specified ID
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetFollowingsAsync(int userId)
        {
            // retrieve and return a list of users that the user with the specified ID is following
            throw new NotImplementedException();
        }
    }
}
