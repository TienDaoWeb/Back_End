using System.Security.Claims;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IJwtService
    {
        string CreateJWTToken(User user, List<string> roles);
        string ExtractEmailFromToken(string token);
        ClaimsPrincipal? VerifyToken(string token);
    }
}
