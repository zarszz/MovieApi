using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.DTOs.V1.Response;
using MovieAPi.Entities;
using MovieAPi.Interfaces.Persistence.Repositories;
using MovieAPi.Interfaces.Persistence.Services;
using ILogger = Microsoft.VisualStudio.Web.CodeGeneration.ILogger;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public class TagServices : ITagServices
    {
        private readonly ITagRepositoryAsync _tagRepositoryAsync;
        private readonly ILogger<Tag> _logger;
        private readonly IValidator<CreateTagDto> _createTagValidator;

        public TagServices(ITagRepositoryAsync tagRepositoryAsync, ILogger<Tag> logger, IValidator<CreateTagDto> createTagValidator)
        {
            _tagRepositoryAsync = tagRepositoryAsync;
            _logger = logger;
            _createTagValidator = createTagValidator;
        }

        public async Task<IActionResult> Create(CreateTagDto createTagDto)
        {
            var validationResult = await _createTagValidator.ValidateAsync(createTagDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "create tag failed"));
            }
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
            var validationResult = await _createTagValidator.ValidateAsync(createTagDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                var messages = errors.Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<List<string>>(messages, "update tag failed"));
            }
            var entity = await _tagRepositoryAsync.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.Log(LogLevel.Error, $"Tag with id: {id} not found");
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
                _logger.Log(LogLevel.Error, $"Tag with id: {id} not found");
                return new NotFoundObjectResult(new Response<string>(false, "Tag not found"));
            }

            await _tagRepositoryAsync.DeleteAsync(entity);
            return new OkObjectResult(new Response<string>(true, "Delete tag successfully"));
        }
    }
}