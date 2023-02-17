namespace MovieAPi.DTOs.V1.Request
{
    public class GetMoviesParams
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
        public string Search { get; set; }
        public int[] Tags { get; set; }
    }
}