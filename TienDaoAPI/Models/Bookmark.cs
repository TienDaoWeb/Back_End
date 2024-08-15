using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    public class Bookmark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? UserId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? User { get; set; }

        [Required]
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }
    }
}
