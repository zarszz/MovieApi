using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Entities;
using MovieAPi.Infrastructures.Persistence.Repositories;
using MovieAPi.Infrastructures.Persistence.Services;
using MovieAPi.Interfaces;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;
using MovieAPi.Middleware;
using MovieAPi.Validator;

namespace MovieAPi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(opts => opts.UseSqlServer(connection));
            
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IUserRepositoryAsync, UserRepositoryAsync>();
            services.AddTransient<ITagRepositoryAsync, TagRepositoryAsync>();
            services.AddTransient<IMovieRepositoryAsync, MovieRepositoryAsync>();
            services.AddTransient<IMovieTagRepositoryAsync, MovieTagRepositoryAsync>();
            services.AddTransient<IStudioRepositoryAsync, StudioRepositoryAsync>();
            services.AddTransient<IMovieScheduleRepositoryAsync, MovieScheduleRepositoryAsync>();
            services.AddTransient<IHttpRequestLogRepositoryAsync, HttpRequestLogRepositoryAsync>();
            services.AddTransient<IOrderRepositoryAsync, OrderRepositoryAsync>();
            #endregion
            
            #region Services
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<ICustomAuthService, CustomAuthService>();
            services.AddTransient<ITagServices, TagServices>();
            services.AddTransient<IMovieServices, MovieServices>();
            services.AddTransient<IStudioServices, StudioServices>();
            services.AddTransient<IMovieScheduleServices, MovieScheduleService>();
            services.AddTransient<IHttpRequestLogService, HttpRequestLogService>();
            services.AddTransient<IOrderServices, OrderServices>();
            #endregion

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            
            services.AddControllers();
            
            services.AddScoped<IValidator<CreateMovieDto>, CreateMovieValidator>();
            services.AddScoped<IValidator<CreateMovieScheduleDto>, CreateMovieScheduleValidator>();
            services.AddScoped<IValidator<CreateStudioDto>, CreateStudioValidator>();
            services.AddScoped<IValidator<CreateTagDto>, CreateTagValidator>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
            services.AddScoped<IValidator<LoginUserDto>, LoginUserValidator>();
            services.AddScoped<IValidator<CreateOrderDto>, CreateOrderValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<LoggingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}