using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterClone.Models
{
    public class Hashtag
    {
        public string Tag { get; set; }
        public IEnumerable<long> TweetIds { get; set; }
    }
}
