using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TwitterClone.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IDatabase _redisCache;

        public MemoryCacheService(IDatabase redisCache = null)
        {
            _redisCache = redisCache;
        }

        public async Task<long> GetTweetCountAsync()
        {
            long tweetCount;
            tweetCount = (long)await _redisCache.StringGetAsync("tweetCount");
            return tweetCount;
        }

        public async Task<long> IncrementTweetCountAsync()
        {
            long tweetCount = await GetTweetCountAsync();
            tweetCount++;

            await _redisCache.StringSetAsync("tweetCount", tweetCount, TimeSpan.FromMinutes(5));
            return tweetCount;
        }
    }
}