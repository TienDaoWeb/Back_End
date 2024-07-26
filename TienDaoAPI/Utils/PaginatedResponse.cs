using System.Net;

namespace TienDaoAPI.Utils
{
    public class PaginatedResponse : Response
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public PaginatedResponse()
        {

        }

        public PaginatedResponse(object? data, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = null;
            IsSuccess = true;
            StatusCode = HttpStatusCode.OK;
        }
    }
}
