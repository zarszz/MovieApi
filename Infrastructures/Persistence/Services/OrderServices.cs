using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.DTOs.V1.Response;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IValidator<CreateOrderDto> _createOrderValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<Order> _logger;
        private readonly IMovieScheduleRepositoryAsync _movieScheduleRepositoryAsync;
        private readonly IOrderRepositoryAsync _orderRepositoryAsync;
        private readonly IUserRepositoryAsync _userRepositoryAsync;

        public OrderServices(IOrderRepositoryAsync orderRepositoryAsync,
            IMovieScheduleRepositoryAsync movieScheduleRepositoryAsync, IUserRepositoryAsync userRepositoryAsync,
            ILogger<Order> logger, IValidator<CreateOrderDto> createOrderValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _orderRepositoryAsync = orderRepositoryAsync;
            _movieScheduleRepositoryAsync = movieScheduleRepositoryAsync;
            _logger = logger;
            _createOrderValidator = createOrderValidator;
            _httpContextAccessor = httpContextAccessor;
            _userRepositoryAsync = userRepositoryAsync;
        }

        public async Task<IActionResult> Create(CreateOrderDto createOrderDto)
        {
            var validationResult = await _createOrderValidator.ValidateAsync(createOrderDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "create Order failed"));
            }

            var currentUser = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var user = await _userRepositoryAsync.GetByIdAsync(Convert.ToInt32(currentUser));
            if (user == null) return new UnauthorizedObjectResult(new Response<string>(false, "Invalid token"));

            var order = new Order
            {
                PaymentMethod = createOrderDto.PaymentMethod,
                User = user,
                UserId = user.Id
            };
            await _orderRepositoryAsync.AddAsync(order);
            var totalItemPrice = 0.0;

            foreach (var orderItem in createOrderDto.OrderItems)
            {
                var schedule = await _movieScheduleRepositoryAsync.GetByIdAsync(orderItem.MovieScheduleId);
                if (schedule == null)
                    return new NotFoundObjectResult(new Response<string>(false, "Movie schedule not found"));

                var newOrderItem = new OrderItem
                {
                    MovieScheduleId = orderItem.MovieScheduleId,
                    MovieSchedule = schedule,
                    Qty = orderItem.Qty,
                    Price = schedule.Price,
                    SubTotalPrice = schedule.Price * orderItem.Qty,
                    Order = order,
                    OrderId = order.Id
                };
                order.OrderItems.Add(newOrderItem);
                totalItemPrice += newOrderItem.SubTotalPrice;
            }

            order.TotalItemPrice = totalItemPrice;

            await _orderRepositoryAsync.UpdateAsync(order);

            return new OkObjectResult(
                new Response<ResponseOrderDto>(true, "create order success"));
        }

        public async Task<IActionResult> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            var currentUser = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var orders =
                await _orderRepositoryAsync.GetPagedResponseAsync(pageNumber, pageSize, Convert.ToInt32(currentUser));
            return new OkObjectResult(new PagedResponse<IEnumerable<ResponseOrderDto>>(
                orders.Select(ResponseOrderDto.FromEntity),
                pageNumber,
                pageSize
            ));
        }

        public async Task<IActionResult> Update(CreateOrderDto createOrderDto, int id)
        {
            var validationResult = await _createOrderValidator.ValidateAsync(createOrderDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "update Order failed"));
            }

            var entity = await _orderRepositoryAsync.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.Log(LogLevel.Error, $"Order with id: {id} not found");
                return new NotFoundObjectResult(new Response<string>(false, "Order not found"));
            }

            entity.PaymentMethod = createOrderDto.PaymentMethod;
            await _orderRepositoryAsync.UpdateAsync(entity);
            return new OkObjectResult(new Response<string>(true, "Update Order successfully"));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _orderRepositoryAsync.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.Log(LogLevel.Error, $"Order with id: {id} not found");
                return new NotFoundObjectResult(new Response<string>(false, "Order not found"));
            }

            await _orderRepositoryAsync.DeleteAsync(entity);
            return new OkObjectResult(new Response<string>(true, "Delete Order successfully"));
        }
    }
}