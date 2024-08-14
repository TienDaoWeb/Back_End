using TienDaoAPI.DTOs;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IReadingService
    {
        public Task<bool> CreateReadingAsync(CreateReadingDTO dto, Chapter chapter);
        public Task<Reading?> GetReadingByIdAsync(int id);
        public Task<IEnumerable<Reading>?> GetReadingsByUserIdAsync(int userId);
        public Task<bool> DeleteReadingAsync(int id);
        public bool Modifiable(Reading reading, UserDTO user);
    }
}
