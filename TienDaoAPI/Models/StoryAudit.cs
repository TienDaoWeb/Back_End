using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("BookAudit")]
    public class BookAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        public DateTime ViewDate { get; set; }

        public int ViewCount { get; set; }

        public int VoteCount { get; set; }
    }
}
