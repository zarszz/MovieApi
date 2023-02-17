using System.Collections.Generic;
using System.Threading.Tasks;
using MovieAPi.DTOs.V1.Request;
using MovieAPi.Entities;

namespace MovieAPi.Interfaces.Persistence.Repositories {
    public interface IMovieScheduleRepositoryAsync : IGenericRepositoryAsync<MovieSchedule>
    {
        public Task<IReadOnlyList<MovieSchedule>> GetPagedResponseAsync(GetMovieScheduleParams getMovieScheduleParams);
    }
}
