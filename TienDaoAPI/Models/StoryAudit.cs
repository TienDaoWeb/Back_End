using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TienDaoAPI.Models
{
    [Table("StoryAudit")]
    public class StoryAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StoryId { get; set; }
        public virtual Story? Story { get; set; }

        public DateTime ViewDate { get; set; }

        public int ViewCount { get; set; }

        public int VoteCount { get; set; }
    }
}
