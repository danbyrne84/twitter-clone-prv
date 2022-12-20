using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Data
{
    public interface ITweetDataAccess
    {
        Task<List<Tweet>> GetTimelineAsync(int maxTweets);
        Task<long> CreateTweetAsync(long tweetId, long userId, string text);
        Task<Tweet> GetTweetAsync(long tweetId);
    }

}
