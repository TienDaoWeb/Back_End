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

        public async Task<Story?> CreateStoryAsync(StoryRequest dto)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.Image;
            await _firebaseStorageService.UploadFile(uniqueFileName, dto.UrlImage);

            Story newStory = new Story
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                Status = dto.Status,
                Image = uniqueFileName,
                Rating = 0,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                GenreId = dto.GenreId
            };

            return await _storyRepository.CreateAsync(newStory);
        }

        public async Task DeleteStoryAsync(Story story)
        {
            await _storyRepository.DeleteAsync(story);
        }

        public async Task<IEnumerable<Story?>> GetAllStoriesAsync(StoryQueryObject queryObj)
        {
            var query = (await _storyRepository.GetAllAsync()).AsQueryable();
            if (!string.IsNullOrEmpty(queryObj.Keyword))
            {
                query = query.Where(s => s.Title.Contains(queryObj.Keyword));
            }
            if (queryObj.Genre > 0)
            {
                query = query.Where(s => s.GenreId == queryObj.Genre);
            }
            var today = DateTime.Today;
            var columnsMap = new Dictionary<string, Expression<Func<Story, object>>>
            {
                ["view_day"] = s => s.storyAudits.First(sw => sw.ViewDate.Date == today.Date),
                ["view_month"] = s => s.storyAudits.Sum(sw => sw.ViewCount * (sw.ViewDate.Month == today.Month && sw.ViewDate.Year == today.Year ? 1 : 0)),
                ["view_week"] = s => s.storyAudits.Sum(sw => sw.ViewCount * (sw.ViewDate >= today.AddDays(-(int)today.DayOfWeek) && sw.ViewDate < today.AddDays(7 - (int)today.DayOfWeek) ? 1 : 0)),
                ["view_count"] = s => s.Views,
                ["rate"] = s => s.Rating,
                ["create_at"] = s => s.CreateDate,
                ["update_at"] = s => s.UpdateDate,
                ["chapter_count"] = s => s.Chapters.Count,
                ["review_count"] = s => s.Reviews.Count,
                ["comment_count"] = s => s.Comments.Count
            };
            if (!string.IsNullOrEmpty(queryObj.SortBy) && columnsMap.ContainsKey(queryObj.SortBy))
            {
                query = queryObj.IsSortAscending ? query.OrderBy(columnsMap[queryObj.SortBy]) : query.OrderByDescending(columnsMap[queryObj.SortBy]);
            }
            if (queryObj.Page <= 0)
            {
                queryObj.Page = 1;
            }
            if (queryObj.PageSize > 20 || queryObj.PageSize <= 0)
            {
                queryObj.PageSize = 20;
                query = query.Skip(queryObj.PageSize * (queryObj.Page - 1)).Take(queryObj.PageSize);
            }

            return query;
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
