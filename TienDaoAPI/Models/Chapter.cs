using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("Chapters")]
    public class Chapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Content { get; set; }

        public int? Index { get; set; }

        public int ViewCount { get; set; } = 0;

        public int WordCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PublishedAt { get; set; }

        [ForeignKey("User")]
        public int OwnerId { get; set; }
        public virtual User? User { get; set; }

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

    }
}
