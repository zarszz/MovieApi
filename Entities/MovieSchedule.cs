using System;

namespace MovieAPi.Entities
{
    public class MovieSchedule : AuditableBaseEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int  StudioId { get; set; }
        public Studio Studio { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}