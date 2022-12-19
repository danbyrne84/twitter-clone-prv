using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Data
{
    public interface IHashtagDataAccess
    {
        Task<List<Tweet>> GetTweetsForHashtagAsync(string hashtag);
    }
}
