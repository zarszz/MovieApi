using System.Threading.Tasks;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;

namespace MovieAPi.Infrastructures.Persistence.Repositories
{
    public class HttpRequestLogRepositoryAsync : IHttpRequestLogRepositoryAsync
    {
        private readonly DatabaseContext _context;
        
        public HttpRequestLogRepositoryAsync(DatabaseContext context)
        {
            _context = context;
        }
        
        public async Task AddAsync(HttpRequestLog httpRequestLog)
        {
            await _context.AddAsync(httpRequestLog);
            await _context.SaveChangesAsync();
        }
    }
}