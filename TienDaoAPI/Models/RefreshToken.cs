using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
