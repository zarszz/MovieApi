using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;

namespace MovieAPi.Infrastructures.Persistence.Repositories
{
    public class OrderRepositoryAsync : IOrderRepositoryAsync
    {
        private readonly DatabaseContext _context;

        public OrderRepositoryAsync(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public Task<IReadOnlyList<Order>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IReadOnlyList<Order>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _context
                .Orders
                .Skip((pageNumber) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<IReadOnlyList<Order>> GetPagedResponseAsync(int pageNumber, int pageSize, int userId)
        {
            return await _context
                .Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(ot => ot.MovieSchedule)
                .ThenInclude(ot => ot.Studio)
                .Include(o => o.OrderItems)
                .ThenInclude(ot => ot.MovieSchedule)
                .ThenInclude(ms => ms.Movie)
                .ThenInclude(mt => mt.MovieTags)
                .ThenInclude(m => m.Tag)
                .Skip((pageNumber) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order> AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Order entity)
        {
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order entity)
        {
            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IOrderRepositoryAsync WithTransaction(DatabaseContext context)
        {
            return new OrderRepositoryAsync(context);
        }
    }
}