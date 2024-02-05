﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TienDaoAPI.IRepositories;
using TienDaoAPI.Models;

namespace TienDaoAPI.Repositories
{
    public class JwtRepository : IJwtRepository
    {
        private readonly IConfiguration _configuration;
        public JwtRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string CreateJWTToken(User user, List<string> roles)
        {
            // Create claim
            var claim = new List<Claim>();
            claim.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }

            // create key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // create credentials 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // create token

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claim,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

