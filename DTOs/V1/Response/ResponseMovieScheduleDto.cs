using System;
using System.Collections.Generic;
using MovieAPi.Entities;

namespace MovieAPi.DTOs.V1.Response
{
    public class ResponseMovieScheduleDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public ResponseMovieDto Movie { get; set; }
        public ResponseStudioDto Studio { get; set; }

        public static ResponseMovieScheduleDto FromEntity(MovieSchedule movieSchedule)
        {
            var responseMovieDto = new ResponseMovieScheduleDto
            {
                Id = movieSchedule.Id,
                StartTime = movieSchedule.StartTime,
                EndTime = movieSchedule.EndTime,
                Date = movieSchedule.Date,
                Price = movieSchedule.Price,
                Movie = ResponseMovieDto.FromEntity(movieSchedule.Movie),
                Studio = ResponseStudioDto.FromEntity(movieSchedule.Studio)
            };
            return responseMovieDto;
        }
    }
}