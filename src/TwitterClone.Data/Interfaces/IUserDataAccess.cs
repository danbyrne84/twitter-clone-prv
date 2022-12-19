using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Data
{
    public interface IUserDataAccess
    {
        Task<List<Tweet>> GetUserTweetsAsync(int userId);
        Task<List<User>> GetFollowersAsync(int userId);
        Task<List<User>> GetFollowingsAsync(int userId);
    }
}
