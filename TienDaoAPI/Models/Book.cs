using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TienDaoAPI.Enums;

namespace TienDaoAPI.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public string? Synopsis { get; set; }

        public string? PosterUrl { get; set; }

        public int LastestIndex { get; set; } = 0;

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }

        [Required]
        [EnumDataType(typeof(BookStatusEnum))]
        public BookStatusEnum Status { get; set; } = BookStatusEnum.Ongoing;

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author? Author { get; set; }

        public int GenreId { get; set; }
        [ForeignKey(nameof(GenreId))]
        public virtual Genre? Genre { get; set; }

        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? User { get; set; }

        public ICollection<Chapter> Chapters { get; set; } = new HashSet<Chapter>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
