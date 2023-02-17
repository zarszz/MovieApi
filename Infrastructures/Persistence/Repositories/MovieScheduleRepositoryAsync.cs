using System.Collections.Generic;
using System.Threading.Tasks;
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
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<MovieSchedule>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<MovieSchedule> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<MovieSchedule>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(MovieSchedule entity)
        {
            throw new System.NotImplementedException();
        }
    }
}