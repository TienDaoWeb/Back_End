namespace TienDaoAPI.DTOs.Responses
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public Guid? RefreshToken { get; set; }
    }
}
