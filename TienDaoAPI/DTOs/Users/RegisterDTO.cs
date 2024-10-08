﻿using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Validation;

namespace TienDaoAPI.DTOs.Users
{
    public class RegisterDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public required string FullName { get; set; }

        [Required]
        [RoleEnumValidation]
        public required string Role { get; set; }
    }
}
