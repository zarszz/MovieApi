namespace MovieAPi.DTOs.V1.Request
{
    public class CreateMovieDto
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Poster { get; set; }
        public string PlayUntil { get; set; }
        public int[] Tags { get; set; }
    }
}