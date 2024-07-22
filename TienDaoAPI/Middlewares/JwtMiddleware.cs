using Microsoft.AspNetCore.Authorization;
using System.Net;
using TienDaoAPI.Helpers;
using TienDaoAPI.Response;

namespace TienDaoAPI.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtHandler _jwtHandler;
        public JwtMiddleware(RequestDelegate next, JwtHandler jwtHandler)
        {
            _next = next;
            _jwtHandler = jwtHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var hasAuthorizeAttribute = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() != null;
            if (hasAuthorizeAttribute)
            {
                var bearerToken = context.Request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(bearerToken))
                {
                    await RespondWithUnauthorized(context, "Missing token!");
                    return;
                }

                var token = bearerToken.Replace("Bearer ", "");
                try
                {
                    var claimsPrincipal = _jwtHandler.VerifyToken(token);
                    if (claimsPrincipal == null)
                    {
                        await RespondWithUnauthorized(context, "Verify token failed!");
                        return;
                    }
                }
                catch (Exception)
                {
                    await RespondWithInteralServerErorr(context, "Invalid token!");
                    return;
                }


            }
            await _next(context);
        }

        private async Task RespondWithUnauthorized(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new CustomResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                IsSuccess = false,
                Message = message
            });
        }

        private async Task RespondWithInteralServerErorr(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new CustomResponse
            {
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccess = false,
                Message = message
            });
        }
    }
}
