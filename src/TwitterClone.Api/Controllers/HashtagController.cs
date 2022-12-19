using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Models;
using TwitterClone.Services;

namespace TwitterClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashtagController : ControllerBase
    {
        private readonly HashtagService _hashtagService;

        public HashtagController(HashtagService hashtagService)
        {
            _hashtagService = hashtagService;
        }

        // GET: api/hashtag/{hashtag}
        [HttpGet("{hashtag}")]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweetsByHashtag(string hashtag)
        {
            var tweets = await _hashtagService.GetTweetsForHashtagAsync(hashtag);

            if (tweets == null)
            {
                return NotFound();
            }

            return tweets;
        }
    }
}