using System.Linq.Expressions;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class StoryService : IStoryService
    {
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IStoryRepository _storyRepository;
        public StoryService(IFirebaseStorageService firebaseStorageService, IStoryRepository storyRepository)
        {
            _firebaseStorageService = firebaseStorageService;
            _storyRepository = storyRepository;
        }

        public async Task<Story?> CreateStoryAsync(StoryRequestDTO storyRequestDTO)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + storyRequestDTO.Image;
            await _firebaseStorageService.UploadFile(uniqueFileName, storyRequestDTO.UrlImage);

            Story newStory = new Story
            {
                Title = storyRequestDTO.Title,
                Author = storyRequestDTO.Author,
                Description = storyRequestDTO.Description,
                Status = storyRequestDTO.Status,
                Image = uniqueFileName,
                Rating = 0,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            return await _storyRepository.CreateAsync(newStory);
        }

        public async Task DeleteStoryAsync(Story story)
        {
            await _storyRepository.DeleteAsync(story);
        }

        public async Task<IEnumerable<Story?>> GetAllStoriesAsync(string keyword, string orderBy, int genre, int page = 1, int size = 10)
        {
            var query = await _storyRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => s.Title.Contains(keyword));
            }
            if (genre > 0)
            {
                query = query.Where(s => s.GenreId == genre);
            }
            if (string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(s => s.Title);
            }
            if (page < 0)
            {
                page = 1;
            }
            if (size > 20 || size <= 0)
            {
                size = 20;
                query = query.Skip(size * (page - 1)).Take(size);
            }

            return query;
        }

        public Task<IEnumerable<Story?>> GetAllStoriesAsync(Expression<Func<Story, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Story?> GetStoryByIdAsync(int storyId)
        {
            return await _storyRepository.GetByIdAsync(storyId);
        }

        public async Task<Story?> UpdateStoryAsync(Story story)
        {
            return await _storyRepository.UpdateAsync(story);
        }


    }
}
