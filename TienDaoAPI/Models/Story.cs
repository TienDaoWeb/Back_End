using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TienDaoAPI.Services.Validation;

namespace TienDaoAPI.Models
{
    [Table("Stories")]
    public class Story
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int Views { get; set; }

        public float Rating { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string? Status { get; set; }

        public int GenreId { get; set; }
        public virtual Genre? Genre { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        //[Column("user_id")]
        //public int UserId { get; set; }
        //[ForeignKey("UserId")]
        //public virtual User? User { get; set; }
        public ICollection<Chapter>? Chapters { get; set; } = new HashSet<Chapter>();
        public ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
    }
}
