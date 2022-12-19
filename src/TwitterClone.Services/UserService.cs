using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Data;
using TwitterClone.Models;

namespace TwitterClone.Services
{
    public class UserService
    {
        private readonly IUserDataAccess _userDataAccess;

        public UserService(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        public async Task<List<Tweet>> GetUserTweetsAsync(int userId)
        {
            return await _userDataAccess.GetUserTweetsAsync(userId);
        }

        public async Task<List<User>> GetFollowersAsync(int userId)
        {
            return await _userDataAccess.GetFollowersAsync(userId);
        }

        public async Task<List<User>> GetFollowingsAsync(int userId)
        {
            return await _userDataAccess.GetFollowingsAsync(userId);
        }
    }

}
