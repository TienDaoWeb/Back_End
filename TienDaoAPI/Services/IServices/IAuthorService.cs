using TienDaoAPI.DTOs.Authors;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IAuthorService
    {
        public Task<Author?> CreateAuthorAsync(AuthorDTO dto);
        public Task<Author?> GetAuthorAsync(string name);
    }
}
