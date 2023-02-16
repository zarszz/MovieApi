using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Response;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        
        public UserServices(IUserRepositoryAsync userRepositoryAsync)
        {
            _userRepositoryAsync = userRepositoryAsync;
        }
        
        public PagedResponse<IEnumerable<ResponseUserDto>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            var users = _userRepositoryAsync.GetPagedResponseAsync(pageNumber, pageSize);
            return new PagedResponse<IEnumerable<ResponseUserDto>>(
                users.Result.Select(ResponseUserDto.FromEntity),
                pageNumber,
                pageSize
            );
        }
    }
}