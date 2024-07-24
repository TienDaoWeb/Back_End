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

        public string? Author { get; set; }

        public string? Description { get; set; }

        public string? PosterUrl { get; set; }

        public int ReviewCount { get; set; } = 0;

        public int ViewCount { get; set; } = 0;

        public int VoteCount { get; set; } = 0;

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [Required]
        [EnumDataType(typeof(BookStatusEnum))]
        public BookStatusEnum Status { get; set; } = BookStatusEnum.Ongoing;

        public int GenreId { get; set; }
        [ForeignKey(nameof(GenreId))]
        public virtual Genre? Genre { get; set; }

        public int? OwnedId { get; set; }
        public virtual User? User { get; set; }

        public ICollection<Chapter> Chapters { get; set; } = new HashSet<Chapter>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<BookAudit> bookAudits { get; set; } = new HashSet<BookAudit>();
    }
}
