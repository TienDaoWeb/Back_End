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

        public int? Order { get; set; }

        [Range(1, 6)]
        public int Emoji { get; set; }

        public DateTime PublishedDate { get; set; }

        public int StoryId { get; set; }
        public virtual Story? Story { get; set; }

    }
}
