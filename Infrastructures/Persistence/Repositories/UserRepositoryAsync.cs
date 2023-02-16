using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;

namespace MovieAPi.Infrastructures.Persistence.Repositories
{
    public class UserRepositoryAsync : IUserRepositoryAsync
    {
        private DatabaseContext _context;
        
        public UserRepositoryAsync(DatabaseContext dbContext)
        {
            _context = dbContext;
        }
        
        public Task<User> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IReadOnlyList<User>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _context
                .Set<User>()
                .Skip((pageNumber) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> AddAsync(User entity)
        {
            await _context.Set<User>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;        
        }

        public Task UpdateAsync(User entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(User entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}