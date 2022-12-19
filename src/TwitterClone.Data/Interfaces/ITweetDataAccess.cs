using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Data
{
    public interface ITweetDataAccess
    {
        Task<List<Tweet>> GetTimelineAsync(int maxTweets);
        Task<int> CreateTweetAsync(string text);
        Task<Tweet> GetTweetAsync(int tweetId);
    }

}
