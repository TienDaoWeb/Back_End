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
        public float Score { get; set; }
        [Required]
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public int BookId { get; set; }
        public virtual required Book Book { get; set; }

        public int ChapterNumber { get; set; } = 0;

        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? User { get; set; }
    }
}
