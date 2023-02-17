using System.Collections.Generic;
using MovieAPi.Entities;

namespace MovieAPi.DTOs.V1.Response
{
    public class ResponseMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Poster { get; set; }
        public string PlayUntil { get; set; }
        public List<ResponseTagDto> Tags { get; set; }

        public static ResponseMovieDto FromEntity(Movie movie)
        {
            var responseMovieDto = new ResponseMovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Overview = movie.Overview,
                Poster = movie.Poster,
                PlayUntil = movie.PlayUntil,
                Tags = new List<ResponseTagDto>()
            };
            foreach (var movieTag in movie.MovieTags)
                responseMovieDto.Tags.Add(new ResponseTagDto
                {
                    Id = movieTag.Tag.Id,
                    Name = movieTag.Tag.Name
                });
            return responseMovieDto;
        }
    }
}