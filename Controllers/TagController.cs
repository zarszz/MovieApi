using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagServices _tagServices;
        public TagController(ITagServices tagServices)
        {
            _tagServices = tagServices;
        }
        
        // GET: api/Tag
        [HttpGet]
        public Task<IActionResult> Get(int page = 0, int size = 10)
        {
            return _tagServices.GetPagedResponseAsync(page, size);
        }

        // GET: api/Tag/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Tag
        [HttpPost]
        public Task<IActionResult> Post(CreateTagDto createTagDto)
        {
            return _tagServices.Create(createTagDto);
        }

        // PUT: api/Tag/5
        [HttpPut("{id}")]
        public Task<IActionResult> Put(int id, CreateTagDto createTagDto)
        {
            return _tagServices.Update(createTagDto, id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(int id)
        {
            return _tagServices.Delete(id);
        }
    }
}
