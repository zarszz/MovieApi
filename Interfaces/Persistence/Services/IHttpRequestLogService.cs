using System.Threading.Tasks;
using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Services
{
    public interface IHttpRequestLogService
    {
        public Task AddAsync(HttpRequestLog httpRequestLog);

    }
}