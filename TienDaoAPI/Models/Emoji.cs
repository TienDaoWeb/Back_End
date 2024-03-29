using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.Models
{
    public class Emoji
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        [Range(1,6)]
        public int TypeEmoji { get; set; }
    }
}
