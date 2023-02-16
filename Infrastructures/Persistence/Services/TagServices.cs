using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.DTOs.V1.Response;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class TagServices : ITagServices
    {
        private readonly ITagRepositoryAsync _tagRepositoryAsync;

        public TagServices(ITagRepositoryAsync tagRepositoryAsync)
        {
            _tagRepositoryAsync = tagRepositoryAsync;
        }

        public async Task<IActionResult> Create(CreateTagDto createTagDto)
        {
            var entity = new Tag
            {
                Name = createTagDto.Name
            };
            var result = await _tagRepositoryAsync.AddAsync(entity);
            return new OkObjectResult(
                new Response<ResponseTagDto>
                {
                    Data = new ResponseTagDto
                    {
                        Id = result.Id,
                        Name = result.Name
                    },
                    Message = "Tag created successfully",
                    Succeeded = true,
                    Errors = null
                });
        }

        public async Task<IActionResult> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            var tags = await _tagRepositoryAsync.GetPagedResponseAsync(pageNumber, pageSize);
            return new OkObjectResult(new PagedResponse<IEnumerable<ResponseTagDto>>(
                tags.Select(ResponseTagDto.FromEntity),
                pageNumber,
                pageSize
            ));
        }

        public async Task<IActionResult> Update(CreateTagDto createTagDto, int id)
        {
            var entity = await _tagRepositoryAsync.GetByIdAsync(id);
            if (entity == null)
            {
                return new NotFoundObjectResult(new Response<string>(false, "Tag not found"));
            }
            entity.Name = createTagDto.Name;
            await _tagRepositoryAsync.UpdateAsync(entity);
            return new OkObjectResult(new Response<string>(true, "Update tag successfully"));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _tagRepositoryAsync.GetByIdAsync(id);
            if (entity == null)
            {
                return new NotFoundObjectResult(new Response<string>(false, "Tag not found"));
            }
            await _tagRepositoryAsync.DeleteAsync(entity);
            return new OkObjectResult(new Response<string>(true, "Delete tag successfully"));

        }
    }
}