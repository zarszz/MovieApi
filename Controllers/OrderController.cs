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
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        
        // GET: api/Order
        [HttpGet]
        public Task<IActionResult> Get(int page = 0, int size = 10)
        {
            return _orderServices.GetPagedResponseAsync(page, size);
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Order
        [HttpPost]
        [Authorize]
        public Task<IActionResult> Post(CreateOrderDto createOrderDto)
        {
            return _orderServices.Create(createOrderDto);
        }
        //
        // // PUT: api/Order/5
        // [HttpPut("{id}")]
        // [Authorize]
        // public Task<IActionResult> Put(int id, CreateOrderDto createOrderDto)
        // {
        //     return _OrderServices.Update(createOrderDto, id);
        // }
        //
        // // DELETE: api/ApiWithActions/5
        // [HttpDelete("{id}")]
        // [Authorize]
        // public Task<IActionResult> Delete(int id)
        // {
        //     return _OrderServices.Delete(id);
        // }
    }
}
