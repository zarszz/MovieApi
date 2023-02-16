using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MovieAPi.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public bool IsAdmin { get; set; }
        
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}