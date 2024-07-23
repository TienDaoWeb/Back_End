using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Net;
using TienDaoAPI.Response;

namespace TienDaoAPI.Helpers
{
    public class SampleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            // If the authorization was forbidden and the resource had a specific requirement,
            // provide a custom 404 response.
            if (authorizeResult.Forbidden)
            {
                await RespondWithForbidden(context, "Bạn không có quyển truy cập tài nguyên này!");
                return;
            }


            if (authorizeResult.Challenged)
            {
                await RespondWithUnathorized(context, "Token không hợp lệ!");
                return;
            }

            // Fall back to the default implementation.
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }

        private async Task RespondWithForbidden(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new CustomResponse
            {
                StatusCode = HttpStatusCode.Forbidden,
                IsSuccess = false,
                Message = message
            });
        }

        private async Task RespondWithUnathorized(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new CustomResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                IsSuccess = false,
                Message = message
            });
        }
    }


}
