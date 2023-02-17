using System.Threading.Tasks;
using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Repositories
{
    public interface IHttpRequestLogRepositoryAsync
    {
        public Task AddAsync(HttpRequestLog httpRequestLog);
    }
}