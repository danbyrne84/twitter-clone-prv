using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Data;
using TwitterClone.Models;

namespace TwitterClone.Services
{
    public class HashtagService
    {
        private readonly IHashtagDataAccess _hashtagDataAccess;

        public HashtagService(IHashtagDataAccess hashtagDataAccess)
        {
            _hashtagDataAccess = hashtagDataAccess;
        }

        public async Task<List<Tweet>> GetTweetsForHashtagAsync(string hashtag)
        {
            // retrieve and return a list of tweets that contain the specified hashtag
            return await _hashtagDataAccess.GetTweetsForHashtagAsync(hashtag);
        }
    }
}
