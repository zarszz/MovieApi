using System.Collections;
using System.Collections.Generic;

namespace MovieAPi.Entities
{
    public class Movie : AuditableBaseEntity
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Poster { get; set; }
        public string PlayUntil { get; set; }
        public ICollection<MovieTag> MovieTags { get; set; }
    }
}