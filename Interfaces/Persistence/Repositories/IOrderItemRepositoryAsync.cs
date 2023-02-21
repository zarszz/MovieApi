using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Repositories
{
    public interface IOrderItemRepositoryAsync : IGenericRepositoryAsync<OrderItem>
    {
        IOrderItemRepositoryAsync WithTransaction(DatabaseContext context);
    }
}