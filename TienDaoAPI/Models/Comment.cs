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

        public List<int> UserLike { get; set; } = [];
        public int? CommentParentId { get; set; } = null;
        public virtual Comment? CommentParent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        public int ChapterNumber { get; set; } = 0;

        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? User { get; set; }
    }
}
