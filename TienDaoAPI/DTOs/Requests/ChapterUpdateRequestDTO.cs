﻿using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Requests
{
    public class ChapterUpdateRequestDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Content { get; set; }
    }
}
