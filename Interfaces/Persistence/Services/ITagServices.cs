using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.DTOs.V1.Response;

namespace MovieAPi.Interfaces.Persistence.Services
{
    public interface ITagServices
    {
        Task<IActionResult> Create(CreateTagDto createTagDto);
        Task<IActionResult> GetPagedResponseAsync(int pageNumber, int pageSize);
        Task<IActionResult> Update(CreateTagDto createTagDto, int id);
        Task<IActionResult> Delete(int id);
    }
}