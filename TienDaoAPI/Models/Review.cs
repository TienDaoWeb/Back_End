using System.ComponentModel.DataAnnotations;
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
        public float RatingCharacter { get; set; }

        [Range(0, 5)]
        public float RatingPlot { get; set; }

        [Range(0, 5)]
        public float RatingWorld { get; set; }

        [Range(0, 5)]
        public float RatingTranslation { get; set; }

        [Required]
        public required string ReviewContent { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int StoryId { get; set; }
        public virtual required Story Story { get; set; }

        public int ChapterNumber { get; set; } = 0;

        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
