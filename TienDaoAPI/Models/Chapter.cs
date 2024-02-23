using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("Chapters")]
    public class Chapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string? Name { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("order")]
        public int? Order { get; set; }

        [Column("emoji")]
        [Range(1, 6)]
        public int Emoji { get; set; }

        [Column("published_date")]
        public DateTime PublishedDate { get; set; }

        [Column("story_id")]
        public int StoryId { get; set; }
        public virtual Story? Story { get; set; }

    }
}
