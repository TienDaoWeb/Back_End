using System.Net;
using System.Security.Claims;
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
            var bearerToken = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(bearerToken))
            {
                await RespondWithUnauthorized(context, "Missing token!");
                return;
            }

            var token = bearerToken.Replace("Bearer ", "");
            var claimsPrincipal = _jwtHandler.VerifyToken(token);
            if (claimsPrincipal == null)
            {
                await RespondWithUnauthorized(context, "Verify token failed!");
                return;
            }
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Add userDto to HttpContext so that it can be accessed in controllers
            context.Items["userId"] = userId;

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
    }
}
