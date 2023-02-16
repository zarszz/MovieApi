using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Interfaces;

namespace MovieAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomAuthService _customAuthService;
        public AuthController(ICustomAuthService customAuthService)
        {
            _customAuthService = customAuthService;
        }
        
        // POST: api/Auth/login
        [HttpPost("login")]
        public Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            return _customAuthService.GenerateAccessToken(loginUserDto);
        }
        
        // POST: api/Auth/register
        [HttpPost("register")]
        public Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            return _customAuthService.RegisterNewUser(registerUserDto);
        }
        
        // POST: api/Auth/logout
        [HttpPost("logout")]
        public string Logout()
        {
            return "logout success";
        }
    }
}
