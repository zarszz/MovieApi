using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Interfaces.Persistence.Services {
    public interface IMovieScheduleServices {
        public Task<IActionResult> Create(CreateMovieScheduleDto createMovieScheduleDto);
        Task<IActionResult> GetPagedResponseAsync(GetMoviesParams getMoviesParams);
        Task<IActionResult> Update(CreateMovieDto createMovieDto, int id);
        Task<IActionResult> Delete(int id);
    }
}