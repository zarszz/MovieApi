using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MovieSchedule> _logger;
        private readonly IValidator<CreateMovieScheduleDto> _createMovieScheduleValidator;

        public MovieScheduleService(IMovieScheduleRepositoryAsync movieScheduleRepositoryAsync,
            IMovieRepositoryAsync movieRepositoryAsync, IStudioRepositoryAsync studioRepositoryAsync,
            ILogger<MovieSchedule> logger, IValidator<CreateMovieScheduleDto> createMovieScheduleValidator)
        {
            _movieScheduleRepositoryAsync = movieScheduleRepositoryAsync;
            _movieRepositoryAsync = movieRepositoryAsync;
            _studioRepositoryAsync = studioRepositoryAsync;
            _logger = logger;
            _createMovieScheduleValidator = createMovieScheduleValidator;
        }

        public async Task<IActionResult> Create(CreateMovieScheduleDto createMovieScheduleDto)
        {
            var validationResult = await _createMovieScheduleValidator.ValidateAsync(createMovieScheduleDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "create movie schedule failed"));
            }
            var schedulePair = await buildMovieSchedule(createMovieScheduleDto);
            if (!(bool)schedulePair["isSuccess"])
            {
                return new NotFoundObjectResult(new Response<string>(false, "Some data not found"));
            }

            await _movieScheduleRepositoryAsync.AddAsync((MovieSchedule)schedulePair["movieSchedule"]);
            var response = new Response<ResponseMovieScheduleDto>(true, "create movie schedule successfully");
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> GetPagedResponseAsync(GetMovieScheduleParams getMovieScheduleParams)
        {
            var movieSchedules = await _movieScheduleRepositoryAsync.GetPagedResponseAsync(getMovieScheduleParams);
            var result = movieSchedules.Select(ResponseMovieScheduleDto.FromEntity).ToList();
            var response = new Response<IQueryable<ResponseMovieScheduleDto>>(result.AsQueryable(),
                "get movie schedules successfully");
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> Update(CreateMovieScheduleDto createMovieScheduleDto, int id)
        {
            var validationResult = await _createMovieScheduleValidator.ValidateAsync(createMovieScheduleDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "update movie schedule failed"));
            }
            var currentSchedule = await _movieScheduleRepositoryAsync.GetByIdAsync(id);
            if (currentSchedule == null)
            {
                _logger.Log(LogLevel.Error,
                    $"[[MovieScheduleService.Update] MovieSchedule with id: {id} not found");
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
                _logger.Log(LogLevel.Error,
                    $"[[MovieScheduleService.Update] MovieSchedule with id: {id} not found");
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
                _logger.Log(LogLevel.Error,
                    $"[[MovieScheduleService.buildMovieSchedule] Movie with id: {createMovieScheduleDto.MovieId} not found");
                return new Dictionary<string, object>
                {
                    { "movieSchedule", null },
                    { "isSuccess", false }
                };
            }

            var studio = await _studioRepositoryAsync.GetByIdAsync(createMovieScheduleDto.StudioId);
            if (studio == null)
            {
                _logger.Log(LogLevel.Error,
                    $"[[MovieScheduleService.buildMovieSchedule] Studio with id: {createMovieScheduleDto.StudioId} not found");
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