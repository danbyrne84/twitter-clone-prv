using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterClone.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
