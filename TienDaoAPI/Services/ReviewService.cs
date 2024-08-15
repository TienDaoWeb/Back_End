using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly TienDaoDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReviewService(IMapper mapper, TienDaoDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateReviewAsync(CreateReviewDTO dto)
        {
            try
            {
                var review = _mapper.Map<Review>(dto);
                _dbContext.Reviews.Add(review);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteReviewAsync(Review review)
        {
            try
            {
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return false;
            }
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> UpdateReviewAsync(Review review, UpdateReviewDTO dto)
        {
            try
            {
                _mapper.Map(dto, review);
                review.UpdatedAt = DateTime.UtcNow;

                _dbContext.Reviews.Update(review);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return false;
            }

        }

        public async Task<ReactionEnum> UserLikeComment(Review review, int userId)
        {
            try
            {
                if (review is not null)
                {
                    var userReact = review.UsersReaction.Exists(userid => userid == userId);
                    if (userReact == true)
                    {
                        review.UsersReaction.Remove(userId);
                        _dbContext.Reviews.Update(review);
                        await _dbContext.SaveChangesAsync();
                        return ReactionEnum.UnLike;
                    }
                    else
                    {
                        review.UsersReaction.Add(userId);
                        _dbContext.Reviews.Update(review);
                        await _dbContext.SaveChangesAsync();
                        return ReactionEnum.Like;
                    }
                }
                else
                {
                    return ReactionEnum.Fail;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return ReactionEnum.Fail;
            }
        }

        public async Task<IEnumerable<Review>?> GetAllReviewAsync(ReviewFilter filter)
        {
            return await _dbContext.Reviews.ToListAsync();
        }
    }
}
