using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class StudioServices : IStudioServices
    {
        private readonly IStudioRepositoryAsync _studioRepositoryAsync;
        private readonly ILogger<Studio> _logger;

        public StudioServices(IStudioRepositoryAsync StudioRepositoryAsync, ILogger<Studio> logger)
        {
            _studioRepositoryAsync = StudioRepositoryAsync;
            _logger = logger;
        }

        public async Task<IActionResult> Create(CreateStudioDto createStudioDto)
        {
            var entity = new Studio
            {
                StudioNumber = createStudioDto.StudioNumber,
                SeatCapacity = createStudioDto.SeatCapacity
            };
            var result = await _studioRepositoryAsync.AddAsync(entity);
            return new OkObjectResult(
                new Response<ResponseStudioDto>
                {
                    Data = new ResponseStudioDto
                    {
                        Id = result.Id,
                        StudioNumber = result.StudioNumber,
                        SeatCapacity = result.SeatCapacity                    },
                    Message = "Studio created successfully",
                    Succeeded = true,
                    Errors = null
                });
        }

        public async Task<IActionResult> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            var studios = await _studioRepositoryAsync.GetPagedResponseAsync(pageNumber, pageSize);
            return new OkObjectResult(new PagedResponse<IEnumerable<ResponseStudioDto>>(
                studios.Select(ResponseStudioDto.FromEntity),
                pageNumber,
                pageSize
            ));
        }

        public async Task<IActionResult> Update(CreateStudioDto createStudioDto, int id)
        {
            var entity = await _studioRepositoryAsync.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.Log(LogLevel.Error,
                    $"[[StudioServices.Update] Studio with id: {id} not found");
                return new NotFoundObjectResult(new Response<string>(false, "Studio not found"));
            }
            entity.StudioNumber = createStudioDto.StudioNumber;
            entity.SeatCapacity = createStudioDto.SeatCapacity;
            await _studioRepositoryAsync.UpdateAsync(entity);
            return new OkObjectResult(new Response<string>(true, "Update Studio successfully"));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _studioRepositoryAsync.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.Log(LogLevel.Error,
                    $"[[StudioServices.Delete] Studio with id: {id} not found");
                return new NotFoundObjectResult(new Response<string>(false, "Studio not found"));
            }
            await _studioRepositoryAsync.DeleteAsync(entity);
            return new OkObjectResult(new Response<string>(true, "Delete Studio successfully"));

        }
    }
}