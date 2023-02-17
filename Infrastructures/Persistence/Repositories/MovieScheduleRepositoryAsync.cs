using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieAPi.DTOs.V1.Request;
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

        public Task<IReadOnlyList<MovieSchedule>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            throw new System.NotImplementedException();
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
            return await _context.MovieSchedules.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyList<MovieSchedule>> GetPagedResponseAsync(
            GetMovieScheduleParams getMovieScheduleParams)
        {
            var query = _context.MovieSchedules
                .Include(m => m.Studio)
                .Include(m => m.Movie)
                .ThenInclude(m => m.MovieTags)
                .ThenInclude(mt => mt.Tag)
                .AsQueryable();

            if (getMovieScheduleParams.MovieId != null)
            {
                query = query.Where(m => m.MovieId == getMovieScheduleParams.MovieId);
            }

            if (getMovieScheduleParams.StudioId != null)
            {
                query = query.Where(m => m.StudioId == getMovieScheduleParams.StudioId);
            }

            if (getMovieScheduleParams.StartTime != null && getMovieScheduleParams.EndTime != null)
            {
                query = query.Where(m =>
                    m.StartTime >= getMovieScheduleParams.StartTime && m.EndTime <= getMovieScheduleParams.EndTime);
            }
            else if (getMovieScheduleParams.StartTime != null)
            {
                query = query.Where(m => m.StartTime >= getMovieScheduleParams.StartTime);
            }
            else if (getMovieScheduleParams.EndTime != null)
            {
                query = query.Where(m => m.EndTime <= getMovieScheduleParams.EndTime);
            }

            if (getMovieScheduleParams.StartPrice != null && getMovieScheduleParams.EndPrice != null)
            {
                query = query.Where(m =>
                    m.Price >= getMovieScheduleParams.StartPrice && m.Price <= getMovieScheduleParams.EndPrice);
            }
            else if (getMovieScheduleParams.StartPrice != null)
            {
                query = query.Where(m => m.Price >= getMovieScheduleParams.StartPrice);
            }
            else if (getMovieScheduleParams.EndPrice != null)
            {
                query = query.Where(m => m.Price <= getMovieScheduleParams.EndPrice);
            }

            if (getMovieScheduleParams.Search != null)
            {
                query = query.Where(m =>
                    m.Movie.Title.Contains(getMovieScheduleParams.Search) ||
                    m.Movie.Overview.Contains(getMovieScheduleParams.Search));
            }

            if (getMovieScheduleParams.Tags != null && getMovieScheduleParams.Tags.Length > 0)
            {
                query = query.Where(m => m.Movie.MovieTags.Any(mt => getMovieScheduleParams.Tags.Contains(mt.TagId)));
            }


            return await query.Skip((getMovieScheduleParams.Page) * getMovieScheduleParams.Size)
                .Take(getMovieScheduleParams.Size)
                .ToListAsync();
        }

        public Task UpdateAsync(MovieSchedule entity)
        {
            _context.MovieSchedules.Update(entity);
            return _context.SaveChangesAsync();
        }
    }
}