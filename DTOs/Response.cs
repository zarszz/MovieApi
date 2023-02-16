using System.Collections.Generic;

namespace MovieAPi.DTOs
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data,string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(bool success, string message)
        {
            Succeeded = success;
            Message = message;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}