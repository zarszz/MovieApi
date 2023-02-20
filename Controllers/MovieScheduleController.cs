using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieScheduleController : ControllerBase
    {
        private readonly IMovieScheduleServices _MovieScheduleServices;
        public MovieScheduleController(IMovieScheduleServices MovieScheduleServices)
        {
            _MovieScheduleServices = MovieScheduleServices;
        }
        
        // GET: api/MovieSchedule
        [HttpGet]
        public Task<IActionResult> Get([FromQuery] GetMovieScheduleParams getMovieScheduleParams)
        {
            return _MovieScheduleServices.GetPagedResponseAsync(getMovieScheduleParams);
        }
        
        // POST: api/MovieSchedule
        [HttpPost]
        [Authorize]
        public Task<IActionResult> Post(CreateMovieScheduleDto createMovieScheduleDto)
        {
            return _MovieScheduleServices.Create(createMovieScheduleDto);
        }

        // PUT: api/MovieSchedule/5
        [HttpPut("{id}")]
        [Authorize]
        public Task<IActionResult> Put(int id, CreateMovieScheduleDto createMovieScheduleDto)
        {
            return _MovieScheduleServices.Update(createMovieScheduleDto, id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public Task<IActionResult> Delete(int id)
        {
            return _MovieScheduleServices.Delete(id);
        }
    }
}
