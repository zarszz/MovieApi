using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.DTOs.V1.Response;
using MovieAPi.Entities;
using MovieAPi.Interfaces;
using MovieAPi.Interfaces.Persistence.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class CustomAuthService : ICustomAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IValidator<RegisterUserDto> _registerUserValidator;
        private readonly IValidator<LoginUserDto> _loginUserValidator;

        public CustomAuthService(IConfiguration configuration, IUserRepositoryAsync userRepository,
            IValidator<RegisterUserDto> registerUserValidator, IValidator<LoginUserDto> loginUserValidator)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _registerUserValidator = registerUserValidator;
            _loginUserValidator = loginUserValidator;
        }

        public async Task<IActionResult> GenerateAccessToken(LoginUserDto loginUserDto)
        {
            var validationResult = await _loginUserValidator.ValidateAsync(loginUserDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "login failed"));
            }
            var user = await GetUser(loginUserDto.Email);
            if (user == null) return new BadRequestObjectResult("invalid email or password");
            if (!validatePassword(loginUserDto.Password, user.Password))
                return new BadRequestObjectResult("invalid email or password");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                claims: claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
            var writeToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new OkObjectResult(new Response<ResponseLoginDto>(new ResponseLoginDto
                {
                    AccessToken = writeToken
                },
                "Login success"));
        }

        public async Task<IActionResult> RegisterNewUser(RegisterUserDto registerUserDto)
        {
            var validationResult = await _registerUserValidator.ValidateAsync(registerUserDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "register failed"));
            }
            var user = new User
            {
                Email = registerUserDto.Email,
                Name = registerUserDto.Name,
                Password = hashPassword(registerUserDto.Password),
                IsAdmin = false,
                Avatar = registerUserDto.Avatar
            };

            var result = await _userRepository.AddAsync(user);
            var response = new Response<ResponseUserDto>
            {
                Data = new ResponseUserDto
                {
                    Avatar = result.Avatar,
                    Email = result.Email,
                    ID = result.Id,
                    Name = result.Name
                },
                Errors = null,
                Message = "Register success",
                Succeeded = true
            };

            return new OkObjectResult(response);
        }

        private async Task<User> GetUser(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        private string hashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        private bool validatePassword(string password, string hashPassword)
        {
            return BC.Verify(password, hashPassword);
        }
    }
}