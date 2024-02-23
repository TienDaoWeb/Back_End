using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("content")]
        [Required]
        public required string Content { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

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
