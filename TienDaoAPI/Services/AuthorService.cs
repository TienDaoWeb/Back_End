using AutoMapper;
using TienDaoAPI.DTOs;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<Author> CreateAuthorAsync(AuthorDTO dto)
        {
            var author = _mapper.Map<Author>(dto);
            var result = await _authorRepository.CreateAsync(author);
            return result!;
        }

        public async Task<Author?> GetAuthorAsync(string name)
        {
            return await _authorRepository.GetAsync(a => a.Name == name);
        }
    }
}
