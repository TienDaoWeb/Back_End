﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TienDaoAPI.DTOs.Authors;

namespace TienDaoAPI.DTOs.Books
{

    public class CreateBookDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public required AuthorDTO Author { get; set; }
        [Required]
        public string Synopsis { get; set; } = string.Empty;
        [JsonIgnore]
        public int OwnerId { get; set; }
        [Required]
        public int GenreId { get; set; }
        public List<int> Tags { get; set; } = new List<int>();
    }
}
