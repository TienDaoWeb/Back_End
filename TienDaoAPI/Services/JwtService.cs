using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string CreateJWTToken(User user, List<string> roles)
        {
            // Create claim
            if (user.Email == null)
            {
                throw new ArgumentNullException("Email must be not null");
            }
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }

            // create key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));

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

        public string ExtractEmailFromToken(string token)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string email = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            return email;
        }
    }
}
