﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Range(0, 5)]
        [Required]
        public float  Score { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<int> UsersReaction { get; set; } = [];

        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual required Book Book { get; set; }
        public int IdReadChapter { get; set; } = 0;
        [ForeignKey("IdReadChapter")]
        public ReadChapter? ReadChapter { get; set; }

        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? User { get; set; }
    }
}
