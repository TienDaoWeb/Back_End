
using System.Net;

namespace TienDaoAPI.Utils
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
