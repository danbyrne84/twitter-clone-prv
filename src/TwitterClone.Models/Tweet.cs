using System;

namespace TwitterClone.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
