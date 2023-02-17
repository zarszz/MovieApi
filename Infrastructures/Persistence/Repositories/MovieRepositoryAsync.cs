using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;

namespace MovieAPi.Infrastructures.Persistence.Repositories
{
    public class MovieRepositoryAsync : IMovieRepositoryAsync
    {
        private readonly DatabaseContext _context;

        public MovieRepositoryAsync(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<IReadOnlyList<Movie>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _context
                .Movies
                .Include(e => e.MovieTags)
                .ThenInclude(m => m.Tag)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Movie>> GetPagedResponseAsync(GetMoviesParams getMoviesParams)
        {
            var query = _context.Movies
                .Include(e => e.MovieTags)
                .ThenInclude(m => m.Tag)
                .AsQueryable();

            if (getMoviesParams.Search != null)
                query = query
                    .Where(e => e.Title.Contains(getMoviesParams.Search) ||
                                e.Overview.Contains(getMoviesParams.Search));

            if (getMoviesParams.Tags != null && getMoviesParams.Tags.Length > 0)
                query = query.Where(e => e.MovieTags.Any(m => getMoviesParams.Tags.Contains(m.TagId)));

            return await query
                .Skip(getMoviesParams.Page * getMoviesParams.Size)
                .Take(getMoviesParams.Size)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Movie> AddAsync(Movie entity)
        {
            await _context.Movies.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Movie entity)
        {
            _context.Movies.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Movie entity)
        {
            _context.Movies.Remove(entity);
            await _context.SaveChangesAsync();
        }
        
        public IMovieRepositoryAsync WithTransaction(DatabaseContext context)
        {
            return new MovieRepositoryAsync(context);
        }
    }
}