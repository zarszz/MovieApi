using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;

namespace MovieAPi.Infrastructures.Persistence.Repositories
{
    public class MovieScheduleRepositoryAsync : IMovieScheduleRepositoryAsync
    {
        private readonly DatabaseContext _context;

        public MovieScheduleRepositoryAsync(DatabaseContext context) 
        {
            _context = context;
        }

        public async Task<MovieSchedule> AddAsync(MovieSchedule entity)
        {
            await _context.MovieSchedules.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task DeleteAsync(MovieSchedule entity)
        {
            _context.MovieSchedules.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<MovieSchedule>> GetAllAsync()
        {
            return await _context.MovieSchedules.ToListAsync();
        }

        public async Task<MovieSchedule> GetByIdAsync(int id)
        {
            return await _context.MovieSchedules.FindAsync(id);
        }

        public async Task<IReadOnlyList<MovieSchedule>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _context.MovieSchedules
                .Include(m => m.Movie)
                .ThenInclude(m => m.MovieTags)
                .Skip((pageNumber) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task UpdateAsync(MovieSchedule entity)
        { 
            _context.MovieSchedules.Update(entity);
            return _context.SaveChangesAsync();
        }
    }
}