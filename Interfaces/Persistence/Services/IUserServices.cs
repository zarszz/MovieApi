using System.Collections.Generic;
using System.Threading.Tasks;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Response;

namespace MovieAPi.Interfaces.Persistence.Services
{
    public interface IUserServices
    {
        PagedResponse<IEnumerable<ResponseUserDto>> GetPagedResponseAsync(int pageNumber, int pageSize);
    }
}