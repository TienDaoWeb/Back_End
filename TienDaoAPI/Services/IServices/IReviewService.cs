﻿using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services.IServices
{
    public interface IReviewService
    {
        public Task<bool> CreateReviewAsync(CreateReviewDTO dto);
        public Task<bool> DeleteReviewAsync(Review review);
        public Task<bool> UpdateReviewAsync(Review review, UpdateReviewDTO dto);
        public Task<ReactionEnum> UserLikeComment(Review review, int userId);
        public Task<IEnumerable<Review>?> GetAllReviewAsync(ReviewFilter filter);
        public Task<Review?> GetReviewByIdAsync(int id);
    }
}