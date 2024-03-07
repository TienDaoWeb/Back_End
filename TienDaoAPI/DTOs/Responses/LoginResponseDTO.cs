namespace TienDaoAPI.DTOs.Response
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
