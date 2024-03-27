using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.Models
{
    public class Emoji
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Heart { get; set; } = 0;
        public int Like { get; set; } = 0;
        public int Fun { get; set; } = 0;
        public int Sad { get; set; } = 0;
        public int Angry { get; set; } = 0;
        public int Attack { get; set; } = 0;
        public int TotalEmoij
        {
            get
            {
                return Heart + Like + Fun + Sad + Angry + Attack;
            }
        }
    }
}
