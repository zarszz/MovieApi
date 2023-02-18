using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.DTOs.V1.Response;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly IMovieRepositoryAsync _movieRepositoryAsync;
        private readonly IMovieTagRepositoryAsync _movieTagRepositoryAsync;
        private readonly ITagRepositoryAsync _tagRepositoryAsync;
        private readonly ILogger<Movie> _logger;
        private readonly IValidator<CreateMovieDto> _createMovieValidator;

        public MovieServices(IMovieRepositoryAsync movieRepositoryAsync,
            IMovieTagRepositoryAsync movieTagRepositoryAsync, ITagRepositoryAsync tagRepositoryAsync,
            ILogger<Movie> logger, IValidator<CreateMovieDto> createMovieValidator)
        {
            _movieRepositoryAsync = movieRepositoryAsync;
            _movieTagRepositoryAsync = movieTagRepositoryAsync;
            _tagRepositoryAsync = tagRepositoryAsync;
            _logger = logger;
            _createMovieValidator = createMovieValidator;
        }

        public async Task<IActionResult> Create(CreateMovieDto createMovieDto)
        {
            var validationResult = await _createMovieValidator.ValidateAsync(createMovieDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "create movie failed"));
            }
            
            var movie = new Movie
            {
                Overview = createMovieDto.Overview,
                Poster = createMovieDto.Poster,
                Title = createMovieDto.Title,
                PlayUntil = createMovieDto.PlayUntil
            };

            var savedMovie = await _movieRepositoryAsync.AddAsync(movie);
            var movieTags = new List<MovieTag>();

            foreach (var movieTagId in createMovieDto.Tags)
            {
                var tag = await _tagRepositoryAsync.GetByIdAsync(movieTagId);
                if (tag == null)
                {
                    _logger.Log(LogLevel.Error,
                        $"[[MovieService.Create] Tag with id: {movieTagId} not found");
                    return new NotFoundObjectResult(new { message = $"MovieTag with id {movieTagId} not found" });
                }

                var movieTag = new MovieTag()
                {
                    Movie = savedMovie,
                    MovieId = savedMovie.Id,
                    Tag = tag,
                    TagId = tag.Id
                };
                movieTags.Add(movieTag);
            }

            var savedMovieTags = _movieTagRepositoryAsync.BulkInsert(movieTags);
            return new OkObjectResult(new Response<IList<MovieTag>>(savedMovieTags, "save new movie success"));
        }

        public async Task<IActionResult> GetPagedResponseAsync(GetMoviesParams getMoviesParams)
        {
            var movies = await _movieRepositoryAsync.GetPagedResponseAsync(getMoviesParams);
            var result = movies.Select(movie => new ResponseMovieDto()
            {
                Id = movie.Id,
                Overview = movie.Overview,
                Poster = movie.Poster,
                Title = movie.Title,
                PlayUntil = movie.PlayUntil,
                Tags = movie.MovieTags.Select(ResponseTagDto.FromMovieTagEntity).ToList()
            });
            return new OkObjectResult(new Response<IList<ResponseMovieDto>>(result.ToList(), "get all movies success"));
        }

        public async Task<IActionResult> Update(CreateMovieDto createMovieDto, int id)
        {
            var validationResult = await _createMovieValidator.ValidateAsync(createMovieDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "update movie failed"));
            }
            
            // begin a transaction
            var context = new DatabaseContext();
            var transaction = await context.Database.BeginTransactionAsync();

            var movie = await _movieRepositoryAsync.WithTransaction(context).GetByIdAsync(id);
            if (movie == null)
            {
                _logger.Log(LogLevel.Error,
                    $"[[MovieService.Update] Movie with id: {id} not found");
                await transaction.RollbackAsync();
                return new NotFoundObjectResult(new Response<string>(false, "movie not found"));
            }

            try
            {
                movie.Overview = createMovieDto.Overview;
                movie.Poster = createMovieDto.Poster;
                movie.Title = createMovieDto.Title;
                movie.PlayUntil = createMovieDto.PlayUntil;

                // update movie
                await _movieRepositoryAsync.WithTransaction(context).UpdateAsync(movie);

                // search movie tag
                var movieTags = new List<MovieTag>();
                foreach (var movieTagId in createMovieDto.Tags)
                {
                    var tag = await _tagRepositoryAsync.WithTransaction(context).GetByIdAsync(movieTagId);
                    if (tag == null)
                    {
                        _logger.Log(LogLevel.Error,
                            $"[[MovieService.Update] Tag with id: {id} not found");
                        await transaction.RollbackAsync();
                        return new NotFoundObjectResult(new { message = $"MovieTag with id {movieTagId} not found" });
                    }

                    var movieTag = new MovieTag()
                    {
                        Movie = movie,
                        MovieId = movie.Id,
                        Tag = tag,
                        TagId = tag.Id
                    };
                    movieTags.Add(movieTag);
                }

                _movieTagRepositoryAsync.WithTransaction(context).RemoveByMovieId(id);
                _movieTagRepositoryAsync.WithTransaction(context).BulkInsert(movieTags);
                await transaction.CommitAsync();
                return new OkObjectResult(new Response<IList<string>>(true, "update movie success"));
            }
            catch (System.Exception e)
            {
                await transaction.RollbackAsync();
                return new BadRequestObjectResult(new Response<string>(false, e.Message));
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepositoryAsync.GetByIdAsync(id);
            if (movie == null)
            {
                _logger.Log(LogLevel.Error,
                    $"[[MovieService.Delete] Movie with id: {id} not found");
                return new NotFoundObjectResult(new Response<string>(false, "movie not found"));
            }

            await _movieRepositoryAsync.DeleteAsync(movie);

            return new OkObjectResult(new Response<string>(true, "delete movie success"));
        }
    }
}