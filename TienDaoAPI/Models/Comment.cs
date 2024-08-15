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

        public int? CommentParentId { get; set; }
        public virtual Comment? CommentParent { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        [Required]
        public int ChapterId { get; set; }
        public virtual Chapter? Chapter { get; set; }

        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? User { get; set; }

        public ICollection<CommentLike> CommentLikes { get; set; } = new HashSet<CommentLike>();
    }
}
