using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterClone.Data;
using TwitterClone.Models;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserDataAccess userDataAccess;

        public UsersController(IUserDataAccess userDataAccess)
        {
            this.userDataAccess = userDataAccess;
        }

        // GET api/users/{userId}/tweets
        [HttpGet("{userId}/tweets")]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetUserTweets(int userId)
        {
            // Retrieve the specified user's tweets
            // Return list of tweets as response
            return await this.userDataAccess.GetUserTweetsAsync(userId);
        }
    }
}
