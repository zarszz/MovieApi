using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;

namespace MovieAPi.Infrastructures.Persistence.Repositories
{
    public class MovieTagRepositoryAsync : IMovieTagRepositoryAsync
    {
        private readonly DatabaseContext _context;

        public MovieTagRepositoryAsync(DatabaseContext context)
        {
            _context = context;
        }

        public Task<Movie> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Movie>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Movie>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public Task<Movie> AddAsync(Movie entity)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(Movie entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(Movie entity)
        {
            throw new System.NotImplementedException();
        }

        public IList<MovieTag> BulkInsert(IList<MovieTag> movieTag)
        {
            _context.BulkInsert(movieTag); 
            _context.SaveChangesAsync();
            return movieTag;
        }

        public async void RemoveByMovieId(int movieId)
        {
            var movieTags = await GetByMovieIdAsync(movieId);
            _context.RemoveRange(movieTags);
            await _context.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<MovieTag>> GetByMovieIdAsync(int movieId)
        {
            return await _context.MovieTags.Where(mt => mt.MovieId == movieId).ToListAsync();
        }
                
        public IMovieTagRepositoryAsync WithTransaction(DatabaseContext context)
        {
            return new MovieTagRepositoryAsync(context);
        }
    }
}