using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    public class BookTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        public int TagId { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
