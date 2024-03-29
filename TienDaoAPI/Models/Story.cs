﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("Stories")]
    public class Story
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public string? Author { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int Views { get; set; }

        public float Rating { get; set; }

        public int Votes { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string? Status { get; set; }


        public int GenreId { get; set; }
        [ForeignKey(nameof(GenreId))]
        public virtual Genre? Genre { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        public ICollection<Chapter> Chapters { get; set; } = new HashSet<Chapter>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<StoryAudit> storyAudits { get; set; } = new HashSet<StoryAudit>();
    }
}
