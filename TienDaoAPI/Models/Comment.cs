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

        public DateTime Time { get; set; }

        [Required]
        public int StoryId { get; set; }
        public virtual required Story Story { get; set; }

        public int ChapterNumber { get; set; } = 0;

        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
