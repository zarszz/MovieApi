using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Repositories
{
    public interface ITagRepositoryAsync : IGenericRepositoryAsync<Tag>
    {
        ITagRepositoryAsync WithTransaction(DatabaseContext context);

    }
}