﻿namespace TienDaoAPI.DTOs.Response
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public Guid? RefreshToken { get; set; }
    }
}
