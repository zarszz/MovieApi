using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Repositories
{
    public interface IStudioRepositoryAsync : IGenericRepositoryAsync<Studio>
    {
        IStudioRepositoryAsync WithTransaction(DatabaseContext context);
    }
}