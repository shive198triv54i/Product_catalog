using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;

namespace ProductCatalog.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto dto)
        {
            var existing = await _uow.Users.GetByEmailAsync(dto.Email);
            if (existing != null) throw new InvalidOperationException("Email already registered.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                // Hash password with BCrypt
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            await _uow.Users.AddAsync(user);
            await _uow.CompleteAsync();

            var token = GenerateToken(user);
            return new AuthResponseDto
            {
                Token = token.token,
                ExpiresAt = token.expiresAt,
                Email = user.Email,
                //Role = user.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _uow.Users.GetByEmailAsync(dto.Email);
            if (user == null) throw new KeyNotFoundException("Invalid credentials.");

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new KeyNotFoundException("Invalid credentials.");

            var token = GenerateToken(user);
            return new AuthResponseDto
            {
                Token = token.token,
                ExpiresAt = token.expiresAt,
                Email = user.Email,
                //Role = user.Role
            };
        }

        // returns tuple (token, expiresAt)
        private (string token, DateTime expiresAt) GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = jwtSettings.GetValue<string>("Key") ?? throw new InvalidOperationException("JWT Key not configured.");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            var expiresMinutes = jwtSettings.GetValue<int>("ExpiresMinutes");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                //new Claim(ClaimTypes.Role, user.Role),
                new Claim("name", user.Name)
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return (token, expiresAt);
        }
    }
}
