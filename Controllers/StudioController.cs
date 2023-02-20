using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudioController : ControllerBase
    {
        private readonly IStudioServices _StudioServices;
        public StudioController(IStudioServices StudioServices)
        {
            _StudioServices = StudioServices;
        }
        
        // GET: api/Studio
        [HttpGet]
        public Task<IActionResult> Get(int page = 0, int size = 10)
        {
            return _StudioServices.GetPagedResponseAsync(page, size);
        }
        
        // POST: api/Studio
        [HttpPost]
        [Authorize]
        public Task<IActionResult> Post(CreateStudioDto createStudioDto)
        {
            return _StudioServices.Create(createStudioDto);
        }

        // PUT: api/Studio/5
        [HttpPut("{id}")]
        [Authorize]
        public Task<IActionResult> Put(int id, CreateStudioDto createStudioDto)
        {
            return _StudioServices.Update(createStudioDto, id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public Task<IActionResult> Delete(int id)
        {
            return _StudioServices.Delete(id);
        }
    }
}
