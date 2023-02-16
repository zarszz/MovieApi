using System.ComponentModel.DataAnnotations;

namespace MovieAPi.Entities
{
    public class MovieTag : BaseEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}