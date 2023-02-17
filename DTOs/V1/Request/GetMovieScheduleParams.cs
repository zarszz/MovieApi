using System;

namespace MovieAPi.DTOs.V1.Request
{
    public class GetMovieScheduleParams : GetMoviesParams
    {
        public int  StudioId { get; set; }
        public int MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }
    }
}