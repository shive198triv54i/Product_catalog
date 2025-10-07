using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using Xunit;

namespace ProductCatalog.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _userRepoMock = new Mock<IUserRepository>();
            _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);

            var configData = new Dictionary<string, string?>
            {
                {"JwtSettings:Key", "ThisIsAVeryLongSecretKeyForJWTTokenGenerationThatIsAtLeast32CharactersLong"},
                {"JwtSettings:Issuer", "TestIssuer"},
                {"JwtSettings:Audience", "TestAudience"},
                {"JwtSettings:ExpiresMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            _authService = new AuthService(_uowMock.Object, configuration);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_AndReturnToken()
        {
            var dto = new UserRegisterDto
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "Password123"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email))
                         .ReturnsAsync((User?)null);

            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                         .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _authService.RegisterAsync(dto);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
            Assert.Equal(dto.Email, result.Email);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEmailAlreadyExists()
        {
            var dto = new UserRegisterDto
            {
                Name = "Jane",
                Email = "jane@example.com",
                Password = "Password123"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email))
                         .ReturnsAsync(new User { Email = dto.Email });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(dto));
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsValid()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123");
            var user = new User
            {
                Id = 1,
                Name = "John",
                Email = "john@example.com",
                Password = hashedPassword
            };

            var dto = new UserLoginDto
            {
                Email = "john@example.com",
                Password = "Password123"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email))
                         .ReturnsAsync(user);

            var result = await _authService.LoginAsync(dto);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
        {
            var dto = new UserLoginDto
            {
                Email = "unknown@example.com",
                Password = "Password123"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email))
                         .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _authService.LoginAsync(dto));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenPasswordInvalid()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123");
            var user = new User
            {
                Id = 1,
                Name = "John",
                Email = "john@example.com",
                Password = hashedPassword
            };

            var dto = new UserLoginDto
            {
                Email = "john@example.com",
                Password = "WrongPassword"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email))
                         .ReturnsAsync(user);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _authService.LoginAsync(dto));
        }
    }
}
