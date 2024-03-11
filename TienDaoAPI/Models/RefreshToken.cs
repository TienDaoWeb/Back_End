using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("RefreshTokens")]
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid? Token { get; set; } = new Guid();
        public DateTime ExpiredAt { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
