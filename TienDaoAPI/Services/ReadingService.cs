using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs.Readings;
using TienDaoAPI.DTOs.Users;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class ReadingService : IReadingService
    {
        private readonly TienDaoDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReadingService(IMapper mapper, TienDaoDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateReadingAsync(CreateReadingDTO dto, Chapter chapter)
        {
            try
            {
                var existingReading = await _dbContext.Readings
                    .Include(r => r.Chapter)
                    .FirstOrDefaultAsync(r => r.Chapter.BookId == chapter.BookId && r.UserId == dto.UserId);
                if (existingReading is not null)
                {
                    existingReading.ChapterId = dto.ChapterId;
                    existingReading.UpdatedAt = DateTime.UtcNow;
                    _dbContext.Readings.Update(existingReading);
                }
                else
                {
                    var newReading = _mapper.Map<Reading>(dto);
                    _dbContext.Readings.Add(newReading);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Reading?> GetReadingByIdAsync(int id)
        {
            return await _dbContext.Readings.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reading>?> GetReadingsByUserIdAsync(int userId)
        {
            return await _dbContext.Readings
                .Include(r => r.Chapter)
                .ThenInclude(c => c.Book)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.UpdatedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteReadingAsync(int id)
        {
            try
            {
                var reading = await _dbContext.Readings.FirstOrDefaultAsync(x => x.Id == id);

                if (reading is null)
                {
                    return false;
                }
                _dbContext.Readings.Remove(reading);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Modifiable(Reading reading, UserDTO user)
        {
            return reading.UserId == user.Id;
        }
    }
}
