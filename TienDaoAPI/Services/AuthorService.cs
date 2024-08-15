using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs.Authors;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly TienDaoDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthorService(IMapper mapper, TienDaoDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Author?> CreateAuthorAsync(AuthorDTO dto)
        {
            try
            {
                var author = _mapper.Map<Author>(dto);
                var result = _dbContext.Authors.Add(author);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Author?> GetAuthorAsync(string name)
        {
            return await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == name);
        }
    }
}
