using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("Stories")]
    public class Story
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("Title")]
        public string? Title { get; set; }

        [Column("author")]
        public string? Author { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("image")]
        public string? Image { get; set; }

        [Column("views")]
        public int Views { get; set; }

        [Column("rating")]
        public float Rating { get; set; }

        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("genre_id")]
        public int GenreId { get; set; }
        public virtual Genre? Genre { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public ICollection<Chapter>? Chapters { get; set; } = new HashSet<Chapter>();
        public ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
    }
}
