using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieServices _movieServices;

        public MovieController(IMovieServices movieServices)
        {
            _movieServices = movieServices;
        }

        // GET: api/Movie
        [HttpGet]
        public Task<IActionResult> Get([FromQuery] GetMoviesParams getMoviesParams)
        {
            return _movieServices.GetPagedResponseAsync(getMoviesParams);
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Movie
        [HttpPost]
        [Authorize]
        public Task<IActionResult> Post(CreateMovieDto createMovieDto)
        {
            return _movieServices.Create(createMovieDto);
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        [Authorize]
        public Task<IActionResult> Put(int id, CreateMovieDto createMovieDto)
        {
            return _movieServices.Update(createMovieDto, id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public Task<IActionResult> Delete(int id)
        {
            return _movieServices.Delete(id);
        }
    }
}
