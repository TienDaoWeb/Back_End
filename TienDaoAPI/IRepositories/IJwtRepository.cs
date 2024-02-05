using TienDaoAPI.Models;

namespace TienDaoAPI.IRepositories
{
    public interface IJwtRepository
    {
        string CreateJWTToken(User user, List<string> roles);
    }
}
