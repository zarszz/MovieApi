using System;

namespace MovieAPi.DTOs.V1.Request {
    public class CreateMovieScheduleDto {
        public int MovieId { get; set; }
        public int  StudioId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}