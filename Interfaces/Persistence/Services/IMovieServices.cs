using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieAPi.DTOs;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.DTOs.V1.Response;

namespace MovieAPi.Interfaces.Persistence.Services
{
    public interface IMovieServices
    {
        Task<IActionResult> Create(CreateMovieDto createMovieDto);
        Task<IActionResult> GetPagedResponseAsync(GetMoviesParams getMoviesParams);
        Task<IActionResult> Update(CreateMovieDto createMovieDto, int id);
        Task<IActionResult> Delete(int id);
    }
}