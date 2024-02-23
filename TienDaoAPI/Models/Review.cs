using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("rating_character")]
        [Range(0, 5)]
        public float RatingCharacter { get; set; }

        [Column("rating_plot")]
        [Range(0, 5)]
        public float RatingPlot { get; set; }

        [Column("rating_world")]
        [Range(0, 5)]
        public float RatingWorld { get; set; }

        [Column("rating_translation")]
        [Range(0, 5)]
        public float RatingTranslation { get; set; }

        [Column("review_content")]
        [Required]
        public required string ReviewContent { get; set; }

        [Column("time")]
        public DateTime CreatedAt { get; set; }

        [Column("story_id")]
        [Required]
        public int StoryId { get; set; }
        public virtual required Story Story { get; set; }

        [Column("chapter_number")]
        public int ChapterNumber { get; set; } = 0;

        [Column("user_id")]
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
