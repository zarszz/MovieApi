using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.DTOs.V1.Response;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class MovieScheduleService : IMovieScheduleServices
    {
        private readonly IMovieScheduleRepositoryAsync _movieScheduleRepositoryAsync;
        private readonly IMovieRepositoryAsync _movieRepositoryAsync;
        private readonly IStudioRepositoryAsync _studioRepositoryAsync;

        public MovieScheduleService(IMovieScheduleRepositoryAsync movieScheduleRepositoryAsync,
            IMovieRepositoryAsync movieRepositoryAsync, IStudioRepositoryAsync studioRepositoryAsync)
        {
            _movieScheduleRepositoryAsync = movieScheduleRepositoryAsync;
            _movieRepositoryAsync = movieRepositoryAsync;
            _studioRepositoryAsync = studioRepositoryAsync;
        }

        public async Task<IActionResult> Create(CreateMovieScheduleDto createMovieScheduleDto)
        {

            var schedulePair = await buildMovieSchedule(createMovieScheduleDto);
            if (!(bool)schedulePair["isSuccess"])
            {
                return new NotFoundObjectResult(new Response<string>(false, "Some data not found"));
            }
            await _movieScheduleRepositoryAsync.AddAsync((MovieSchedule) schedulePair["movieSchedule"]);
            var response = new Response<ResponseMovieScheduleDto>(true, "create movie schedule successfully");
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> GetPagedResponseAsync(GetMovieScheduleParams getMovieScheduleParams)
        {
            var movieSchedules = await _movieScheduleRepositoryAsync.GetPagedResponseAsync(getMovieScheduleParams.Page,
                getMovieScheduleParams.Size);
            var result = movieSchedules.Select(ResponseMovieScheduleDto.FromEntity).ToList();
            var response = new Response<IQueryable<ResponseMovieScheduleDto>>(result.AsQueryable(),
                "get movie schedules successfully");
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> Update(CreateMovieScheduleDto createMovieScheduleDto, int id)
        {
            var currentSchedule = await _movieScheduleRepositoryAsync.GetByIdAsync(id);
            if (currentSchedule == null)
            {
                return new NotFoundObjectResult(new Response<string>(false, "Movie schedule not found"));
            }
            
            var schedulePair = await buildMovieSchedule(createMovieScheduleDto);
            if (!(bool)schedulePair["isSuccess"])
            {
                return new NotFoundObjectResult(new Response<string>(false, "Some data not found"));
            }
            
            var schedule = (MovieSchedule)schedulePair["movieSchedule"];
            schedule.Id = currentSchedule.Id;
            await _movieScheduleRepositoryAsync.UpdateAsync(schedule);
            
            return new OkObjectResult(new Response<string>(true, "Update movieSchedule successfully"));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _movieScheduleRepositoryAsync.GetByIdAsync(id);
            if (entity == null)
            {
                return new NotFoundObjectResult(new Response<string>(false, "Movie schedule not found"));
            }

            await _movieScheduleRepositoryAsync.DeleteAsync(entity);
            return new OkObjectResult(new Response<string>(true, "Delete Movie schedule  successfully"));
        }

        private async Task<Dictionary<string, object>> buildMovieSchedule(CreateMovieScheduleDto createMovieScheduleDto)
        {
            var movie = await _movieRepositoryAsync.GetByIdAsync(createMovieScheduleDto.MovieId);
            if (movie == null)
            {
                // TODO Log here
                return new Dictionary<string, object>
                {
                    { "movieSchedule", null },
                    { "isSuccess", false }
                };
            }

            var studio = await _studioRepositoryAsync.GetByIdAsync(createMovieScheduleDto.StudioId);
            if (studio == null)
            {
                // TODO Log here
                return new Dictionary<string, object>
                {
                    { "movieSchedule", null },
                    { "isSuccess", false }
                };
            }

            var movieSchedule = new MovieSchedule
            {
                Studio = studio,
                StudioId = studio.Id,
                Movie = movie,
                MovieId = movie.Id,
                StartTime = createMovieScheduleDto.StartTime,
                EndTime = createMovieScheduleDto.EndTime,
                Price = createMovieScheduleDto.Price,
                Date = createMovieScheduleDto.Date
            };
            return new Dictionary<string, object>
            {
                { "movieSchedule", movieSchedule },
                { "isSuccess", true }
            };
        }
    }
}