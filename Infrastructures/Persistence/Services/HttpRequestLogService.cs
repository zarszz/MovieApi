using System.Threading.Tasks;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class HttpRequestLogService : IHttpRequestLogService
    {
        private readonly IHttpRequestLogRepositoryAsync _httpRequestLogRepositoryAsync;

        public HttpRequestLogService(IHttpRequestLogRepositoryAsync httpRequestLogRepositoryAsync)
        {
            _httpRequestLogRepositoryAsync = httpRequestLogRepositoryAsync;
        }

        public Task AddAsync(HttpRequestLog httpRequestLog)
        {
            return _httpRequestLogRepositoryAsync.AddAsync(httpRequestLog);
        }
    }
}