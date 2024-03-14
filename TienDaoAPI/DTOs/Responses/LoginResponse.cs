namespace TienDaoAPI.DTOs.Response
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public Guid? RefreshToken { get; set; }
    }
}
