using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Models;

namespace TienDaoAPI.DTOs
{
    public class UpdateCommentDTO
    {
        [Required]
        public required string Content { get; set; }
    }
}
