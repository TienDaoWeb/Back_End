using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Content { get; set; }

        public int LikeCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        public int BookId { get; set; }
        public virtual required Book Book { get; set; }

        public int ChapterNumber { get; set; } = 0;

        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
