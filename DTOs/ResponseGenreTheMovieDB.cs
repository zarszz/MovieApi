using System.Collections.Generic;

namespace MovieAPi.DTOs
{

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ResponseGenreTheMovieDB
    {
        public List<Genre> genres { get; set; }
    }

}