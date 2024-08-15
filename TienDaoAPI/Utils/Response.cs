using Microsoft.Identity.Client;
using System.Net;

namespace TienDaoAPI.Utils
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }
        public object? Data { get; set; }

        public Response SetData(object data)
        {
            Data = data;
            return this;
        }

        public Response SetMessage(string message)
        {
            Message = message;
            return this;
        }

        public Response InternalServerError()
        {
            StatusCode = HttpStatusCode.InternalServerError;
            Message = "Máy chủ của chúng tôi đang gặp lỗi. Hãy kiên nhẫn chờ đợi trong khi chúng tôi sửa chữa!";
            return this;
        }

        public Response Forbidden()
        {
            StatusCode = HttpStatusCode.Forbidden;
            Message = "Bạn không có đủ quyền để thực hiện hành động này!";
            return this;
        }

        public Response NotFound()
        {
            StatusCode = HttpStatusCode.NotFound;
            return this;
        }
        public Response ForBidden()
        {
            StatusCode = HttpStatusCode.Forbidden;
            return this;
        } 

        public Response BadRequest()
        {
            StatusCode = HttpStatusCode.BadRequest;
            return this;
        }

        public Response Success()
        {
            StatusCode = HttpStatusCode.OK;
            IsSuccess = true;
            return this;
        }
    }
}
