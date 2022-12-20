using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterClone.Models
{
    public class Follower
    {
        public int UserId { get; set; }
        public int FollowerId { get; set; }
        public string Id { get; set; }
    }

}
