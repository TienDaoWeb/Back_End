using AutoMapper;
using TienDaoAPI.DTOs;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class ReadingService : IReadingService
    {
        private readonly IReadingRepository _readingChapterRepository;
        private readonly IMapper _mapper;

        public ReadingService(IReadingRepository readingChapterRepository, IMapper mapper)
        {
            _readingChapterRepository = readingChapterRepository;
            _mapper = mapper;
        }

        public async Task<Reading?> CreateReadingAsync(CreateReadingDTO dto, Chapter chapter)
        {
            try
            {
                var existingReading = await _readingChapterRepository.GetAsync(r => r.Chapter.BookId == chapter.BookId && r.UserId == dto.UserId, "Chapter");
                if (existingReading != null)
                {
                    existingReading.ChapterId = dto.ChapterId;
                    existingReading.UpdatedAt = DateTime.UtcNow;
                    return await _readingChapterRepository.UpdateAsync(existingReading);
                }
                var newReading = _mapper.Map<Reading>(dto);
                return await _readingChapterRepository.CreateAsync(newReading);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Reading?> GetReadingByIdAsync(int id)
        {
            return await _readingChapterRepository.GetAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reading>?> GetReadingsByUserIdAsync(int userId)
        {
            return await _readingChapterRepository.FilterAsync(r => r.UserId == userId, "Chapter,Chapter.Book", q => q.OrderByDescending(r => r.UpdatedAt));
        }

        public async Task<bool> DeleteReadingAsync(int id)
        {
            try
            {
                await _readingChapterRepository.DeleteByIdAsync(id);
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
