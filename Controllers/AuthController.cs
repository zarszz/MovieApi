using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // POST: api/Auth/login
        [HttpPost("login")]
        public string Login()
        {
            return "login success";
        }
        
        // POST: api/Auth/logout
        [HttpPost("logout")]
        public string Logout()
        {
            return "logout success";
        }
    }
}
