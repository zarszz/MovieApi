using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieAPi.DTOs;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.QueueService;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public sealed class GetPopularMovieService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<GetPopularMovieService> _logger;
        private readonly CancellationToken _cancellationToken;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetPopularMovieService(IBackgroundTaskQueue taskQueue, ILogger<GetPopularMovieService> logger,
            IHostApplicationLifetime applicationLifetime, IServiceScopeFactory scopeFactory)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
            _serviceScopeFactory = scopeFactory;
        }

        public void StartMonitorLoop()
        {
            _logger.LogInformation($"{nameof(GetPopularMovieService)} loop is starting....");

            // Run the loop in a background thread
            Task.Run(async () => await MonitorAsync(), _cancellationToken);
        }

        private async ValueTask MonitorAsync()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                // enqueue a background work item
                await _taskQueue.QueueBackgroundWorkItemAsync(BuildWorkItemAsync);
            }
        }

        private async ValueTask BuildWorkItemAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            // simulate three 5-second tasks to complete
            // for each enqueued work item
            var guid = Guid.NewGuid();
            _logger.LogInformation("Queue work item {Guid} is starting.", guid);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Run(async () =>
                    {
                        var now = DateTime.Now;
                        _logger.LogInformation($"[{now.ToString(CultureInfo.CurrentCulture)}] Executing task {guid} in background queue.");
                        var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                        _logger.LogInformation("======= GET POPULAR MOVIE FROM API =======");
                        var movies = await MovieDbClientService.BuildGetRequest<ResponseTheMovieDB>("/movie/popular?api_key=95467e28a39b346de61f7c8f8f3f6cea&language=en-US&page=1&sort=desc");
                        var genres = await MovieDbClientService.BuildGetRequest<ResponseGenreTheMovieDB>("/genre/movie/list?api_key=95467e28a39b346de61f7c8f8f3f6cea&language=en-US&page=1&sort=desc");
                        foreach (var movie in movies.results)
                        {
                            // check if movie already exist in database by id, if exist create one otherwise just log
                            var movieExist = await databaseContext.Movies.AnyAsync(m => m.Title.Contains(movie.title));
                            if (!movieExist)
                            {
                                var newMovie = new Movie
                                {
                                    Title = movie.title,
                                    Overview = movie.overview,
                                    Poster = movie.poster_path,
                                    PlayUntil = DateTime.Now.AddDays(10).ToString(CultureInfo.InvariantCulture),
                                };
                                await databaseContext.Movies.AddAsync(newMovie);
                                await databaseContext.SaveChangesAsync();

                                var tags = new List<MovieTag>();

                                // check if genre/tags is already exist in database by id from api, if exist insert, otherwise create new
                                foreach (var genre in movie.genre_ids)
                                {
                                    var currentMovieTag = new MovieTag();

                                    var currentGenre = genres.genres.Find(g => g.id == genre);
                                    var genreExist =
                                        await databaseContext.Tags.FirstOrDefaultAsync(
                                            g => g.Name.Contains(currentGenre.name));

                                    if (genreExist != null)
                                    {
                                        var existingGenre = await databaseContext.Tags.FirstOrDefaultAsync(g =>
                                            g.Name.Contains(currentGenre.name));
                                        currentMovieTag.TagId = existingGenre.Id;
                                        currentMovieTag.MovieId = newMovie.Id;
                                        tags.Add(currentMovieTag);
                                        continue;
                                    }

                                    ;

                                    if (currentGenre != null)
                                    {
                                        var newGenre = new Tag
                                        {
                                            Name = currentGenre.name,
                                        };
                                        await databaseContext.Tags.AddAsync(newGenre);
                                        await databaseContext.SaveChangesAsync();
                                        //log if new genre is inserted
                                        _logger.LogInformation("New genre with id {Id} successfully added to database",
                                            genre);

                                        currentMovieTag.TagId = newGenre.Id;
                                    }

                                    currentMovieTag.MovieId = newMovie.Id;
                                    tags.Add(currentMovieTag);
                                }

                                // save to db with context
                                await databaseContext.BulkInsertAsync(tags);
                                await databaseContext.SaveChangesAsync();

                                // log if operation success
                                _logger.LogInformation("Movie with id {Id} successfully added to database", movie.id);
                            }
                            else
                            {
                                _logger.LogInformation("Movie with id {Id} already exist in database", movie.id);
                            }
                        }

                        _logger.LogInformation("======= END GET POPULAR MOVIE FROM API =======");
                        var after = DateTime.Now;
                        _logger.LogInformation($"[{after}] Executing task {guid} is completed. Time elapsed: {now.Subtract(after).TotalSeconds} seconds");
                        _logger.LogInformation($"[{after}] Next task will be executed in {after.Add(TimeSpan.FromSeconds(100))}");
                        await Task.Delay(TimeSpan.FromSeconds(60 * 60 * 12), cancellationToken);
                    }, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // prevent throwing
                }
                _logger.LogInformation("Queue work item {Guid} is running", guid);
            }
            _logger.LogInformation("Queue work item {Guid} completed.", guid);
        }
    }
}