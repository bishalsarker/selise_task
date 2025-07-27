using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SeliseTaskManager.Domain.User;
using SeliseTaskManager.Infrastructure.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SeliseTaskManager.Infrastructure.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Guid userId, string email, UserRoles role)
        {
            var key = 
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["SecuritySettings:JwtSettings:key"] ?? string.Empty));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, $"{(int)role}"),
            };

            var token = new JwtSecurityToken(
                issuer: _config["SecuritySettings:JwtSettings:issuer"],
                audience: _config["SecuritySettings:JwtSettings:audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["SecuritySettings:JwtSettings:tokenExpirationInMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
