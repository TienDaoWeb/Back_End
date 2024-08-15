using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Models;

namespace TienDaoAPI.DTOs.Comments
{
    public class UpdateCommentDTO
    {
        [Required]
        public required string Content { get; set; }
    }
}
