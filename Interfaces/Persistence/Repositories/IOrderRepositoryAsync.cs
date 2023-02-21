using System.Collections.Generic;
using System.Threading.Tasks;
using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Repositories
{
    public interface IOrderRepositoryAsync : IGenericRepositoryAsync<Order>
    {
        IOrderRepositoryAsync WithTransaction(DatabaseContext context);
        public Task<IReadOnlyList<Order>> GetPagedResponseAsync(int pageNumber, int pageSize, int userId);
    }
}