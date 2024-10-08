﻿using TienDaoAPI.Enums;

namespace TienDaoAPI.DTOs.Users
{
    public class UpdateProfileDTO
    {
        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? Birthday { get; set; }

        public GenderEnum? Gender { get; set; }
    }
}
