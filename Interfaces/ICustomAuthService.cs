using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Interfaces
{
    public interface ICustomAuthService
    {
        public Task<IActionResult> GenerateAccessToken(LoginUserDto loginUserDto);
        
        public Task<IActionResult> RegisterNewUser(RegisterUserDto registerUserDto);
    }
}