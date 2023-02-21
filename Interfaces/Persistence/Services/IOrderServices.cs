using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Interfaces.Persistence.Services
{
    public interface IOrderServices
    {
        Task<IActionResult> Create(CreateOrderDto dto);
        Task<IActionResult> GetPagedResponseAsync(int pageNumber, int pageSize);
        Task<IActionResult> Update(CreateOrderDto dto, int id);
        Task<IActionResult> Delete(int id);
    }
}