using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterClone.Models;
using TwitterClone.Services;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private TweetService tweetService;

        public TweetsController(TweetService tweetService)
        {
            this.tweetService = tweetService;
        }

        // GET api/tweets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tweet>>> Get([FromQuery] int max_tweets)
        {
            // Retrieve the current user's timeline
            // Return list of tweets as response
            return await tweetService.GetTimelineAsync(max_tweets);
        }

        // POST api/tweets
        [HttpPost]
        public async Task<ActionResult<Tweet>> CreateTweet([FromBody] Tweet tweet)
        {
            // Create a new tweet
            // Return newly created tweet as response
            var tweetId = await tweetService.CreateTweetAsync(tweet.UserId, tweet.Text);

            // this will do a double hit to the DB, optimization: just return the ID
            return await tweetService.GetTweetAsync(tweetId);
        }

        // GET api/tweets/{tweetId}
        [HttpGet("{tweetId}")]
        public async Task<ActionResult<Tweet>> GetTweet(int tweetId)
        {
            // Retrieve tweet with the specified ID
            // Return tweet as response

            return await tweetService.GetTweetAsync(tweetId);
        }
    }
}
