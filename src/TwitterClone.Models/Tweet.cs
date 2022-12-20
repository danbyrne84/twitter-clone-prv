using System;

namespace TwitterClone.Models
{
    public class Tweet
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public long UserId { get; set; }
    }
}
