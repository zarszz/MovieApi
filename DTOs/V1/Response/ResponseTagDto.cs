using MovieAPi.Entities;

namespace MovieAPi.DTOs.V1.Response
{
    public class ResponseTagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public static ResponseTagDto FromEntity(Tag tag)
        {
            return new ResponseTagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }
        
        public static ResponseTagDto FromMovieTagEntity(MovieTag tag)
        {
            return new ResponseTagDto
            {
                Id = tag.Tag.Id,
                Name = tag.Tag.Name
            };
        }
    }
}