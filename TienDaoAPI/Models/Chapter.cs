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

        public DateTime PublishedAt { get; set; }

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

    }
}
