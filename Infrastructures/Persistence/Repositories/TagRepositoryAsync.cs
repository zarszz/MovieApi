using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;

namespace MovieAPi.Infrastructures.Persistence.Repositories
{
    public class TagRepositoryAsync : ITagRepositoryAsync
    {
        private readonly DatabaseContext _context;
        public TagRepositoryAsync(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }
        
        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public Task<IReadOnlyList<Tag>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IReadOnlyList<Tag>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _context
                .Tags
                .Skip((pageNumber) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();        }

        public async Task<Tag> AddAsync(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;         }

        public async Task UpdateAsync(Tag entity)
        {
            _context.Tags.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tag entity)
        {
            _context.Tags.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}