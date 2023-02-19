using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MovieAPi.DTOs;

namespace MovieAPi.Infrastructures.Persistence.Services
{
    public static class MovieDbClientService
    {
        static readonly HttpClient Client = new HttpClient();
        private const string BaseUrl = "https://api.themoviedb.org/3";

        public static async Task<T> BuildGetRequest<T>(string url)
        {
            try
            {
                using var response = await Client.GetAsync(BaseUrl + url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return default;
        }
    }
}