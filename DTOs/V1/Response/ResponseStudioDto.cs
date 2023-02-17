using MovieAPi.Entities;

namespace MovieAPi.DTOs.V1.Response
{
    public class ResponseStudioDto
    {
        public int Id { get; set; }
        public int StudioNumber { get; set; }
        public int SeatCapacity { get; set; }
        
        public static ResponseStudioDto FromEntity(Studio studio)
        {
            return new ResponseStudioDto
            {
                Id = studio.Id,
                StudioNumber = studio.StudioNumber,
                SeatCapacity = studio.SeatCapacity
            };
        }
    }
}