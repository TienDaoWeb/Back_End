using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TienDaoAPI.Helpers;
using TienDaoAPI.Response;

namespace TienDaoAPI.Attributes
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly JwtHandler _jwtHandler;

        public CustomAuthorizeAttribute(JwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAuthorizeAttribute = context.ActionDescriptor.EndpointMetadata
            .Any(em => em is AuthorizeAttribute);
            if (hasAuthorizeAttribute)
            {
                var bearerToken = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();

                if (string.IsNullOrEmpty(bearerToken))
                {
                    context.Result = new JsonResult(new CustomResponse
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        IsSuccess = false,
                        Message = "Missing token!"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }

                try
                {
                    var token = bearerToken.Replace("Bearer ", "");
                    var claimsPrincipal = _jwtHandler.VerifyToken(token);

                    if (claimsPrincipal == null)
                    {
                        context.Result = new JsonResult(new CustomResponse
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            IsSuccess = false,
                            Message = "Verify token failed!"
                        })
                        {
                            StatusCode = (int)HttpStatusCode.Unauthorized
                        };
                        return;
                    }
                    context.HttpContext.User = claimsPrincipal;
                }
                catch (Exception)
                {
                    context.Result = new JsonResult(new CustomResponse
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        IsSuccess = false,
                        Message = "Invalid token!"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }
            }
        }
    }
}
