using System.Collections.Generic;
using System.Threading.Tasks;
using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Repositories
{
    public interface IMovieTagRepositoryAsync : IGenericRepositoryAsync<Movie>
    {
        IList<MovieTag> BulkInsert(IList<MovieTag> movieTag);
        void RemoveByMovieId(int movieId);
        Task<IReadOnlyList<MovieTag>> GetByMovieIdAsync(int movieId);
        IMovieTagRepositoryAsync WithTransaction(DatabaseContext context);
    }
}