using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;

namespace MovieAPi.Infrastructures.Persistence.Repositories
{
    public class StudioRepositoryAsync : IStudioRepositoryAsync
    {
        private readonly DatabaseContext _context;

        public StudioRepositoryAsync(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<Studio> GetByIdAsync(int id)
        {
            return await _context.Studios.FindAsync(id);
        }

        public Task<IReadOnlyList<Studio>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IReadOnlyList<Studio>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _context
                .Studios
                .Skip((pageNumber) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Studio> AddAsync(Studio entity)
        {
            await _context.Studios.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Studio entity)
        {
            _context.Studios.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Studio entity)
        {
            _context.Studios.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IStudioRepositoryAsync WithTransaction(DatabaseContext context)
        {
            return new StudioRepositoryAsync(context);
        }
    }
}