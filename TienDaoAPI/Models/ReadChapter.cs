using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("ReadChapters")]
    public class ReadChapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int ChapterId { get; set; }
        public required Chapter Chapter { get; set; }

        public DateTime ReadTime { get; set; }
    }
}
