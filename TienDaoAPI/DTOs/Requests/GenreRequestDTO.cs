﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Requests
{
    public class GenreRequestDTO
    {
        [Required]
        public string? Name { get; set; } = string.Empty;
        [Required]
        public string? Description { get; set; } = string.Empty;
    }
}
