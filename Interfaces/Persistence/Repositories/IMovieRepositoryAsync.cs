using System.Collections.Generic;
using System.Threading.Tasks;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Repositories
{
    public interface IMovieRepositoryAsync : IGenericRepositoryAsync<Movie>
    {
        Task<IReadOnlyList<Movie>> GetPagedResponseAsync(GetMoviesParams getMoviesParams);
        IMovieRepositoryAsync WithTransaction(DatabaseContext context);
    }
}