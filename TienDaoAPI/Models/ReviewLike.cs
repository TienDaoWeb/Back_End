using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("ReviewLikes")]
    public class ReviewLike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? User { get; set; }

        [Required]
        public int ReviewId { get; set; }
        public virtual Review? Review { get; set; }

    }
}
