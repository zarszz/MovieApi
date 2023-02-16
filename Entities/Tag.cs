using System.Collections.Generic;

namespace MovieAPi.Entities
{
    public class Tag : AuditableBaseEntity
    {
        public string Name { get; set; }
        public ICollection<MovieTag> MovieTags { get; set; }
    }
}