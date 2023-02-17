using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class MovieScheduleService : IMovieScheduleServices
    {
        private readonly IMovieScheduleRepositoryAsync _movieScheduleRepositoryAsync;
        private readonly IMovieRepositoryAsync _movieRepositoryAsync;

        public MovieScheduleService(IMovieScheduleRepositoryAsync movieScheduleRepositoryAsync, IMovieRepositoryAsync movieRepositoryAsync)
        {
            _movieScheduleRepositoryAsync = movieScheduleRepositoryAsync;
            _movieRepositoryAsync = movieRepositoryAsync;
        }

        public async Task<IActionResult> Create(CreateMovieScheduleDto createMovieScheduleDto)
        {
            var movie = await _movieRepositoryAsync.GetByIdAsync(createMovieScheduleDto.MovieId);
            if (movie == null)
            {
                return new NotFoundObjectResult(new Response<string>(false, "Movie not found"));
            }

            return new OkObjectResult(new Response<string>(true, "ok"));
        }
    }
}