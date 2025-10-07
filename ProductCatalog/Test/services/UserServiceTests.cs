using AutoMapper;
using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using FluentAssertions;

namespace Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _service = new UserService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateUserSuccessfully()
        {
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Role = "User"
            };

            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Role = "User"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _service.CreateAsync(userDto);

            result.Should().NotBeNull();
            result.Name.Should().Be("Test User");
            result.Email.Should().Be("test@example.com");
            userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnUser()
        {
            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Role = "User"
            };

            var userDto = new UserDto
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Role = "User"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test User");
            result.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<UserDto>(null)).Returns((UserDto?)null);

            var result = await _service.GetByIdAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ShouldReturnUser()
        {
            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Role = "User"
            };

            var userDto = new UserDto
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Role = "User"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _service.GetByEmailAsync("test@example.com");

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test User");
            result.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task GetByEmailAsync_WithInvalidEmail_ShouldReturnNull()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByEmailAsync("nonexistent@example.com")).ReturnsAsync((User?)null);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<UserDto>(null)).Returns((UserDto?)null);

            var result = await _service.GetByEmailAsync("nonexistent@example.com");

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "User 1", Email = "user1@example.com", Password = "pass1", Role = "User" },
                new User { Id = 2, Name = "User 2", Email = "user2@example.com", Password = "pass2", Role = "Admin" }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto { Id = 1, Name = "User 1", Email = "user1@example.com", Role = "User" },
                new UserDto { Id = 2, Name = "User 2", Email = "user2@example.com", Role = "Admin" }
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(u => u.Email == "user1@example.com");
            result.Should().Contain(u => u.Email == "user2@example.com");
        }

        [Fact]
        public async Task UpdateAsync_WithValidUser_ShouldUpdateSuccessfully()
        {
            var userDto = new UserDto
            {
                Id = 1,
                Name = "Updated User",
                Email = "updated@example.com",
                Role = "Admin"
            };

            var existingUser = new User
            {
                Id = 1,
                Name = "Original User",
                Email = "original@example.com",
                Password = "originalpass",
                Role = "User"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingUser);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            await _service.UpdateAsync(userDto);

            userRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidUserId_ShouldThrowKeyNotFoundException()
        {
            var userDto = new UserDto
            {
                Id = 999,
                Name = "Updated User",
                Email = "updated@example.com",
                Role = "Admin"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(userDto));
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteSuccessfully()
        {
            var user = new User
            {
                Id = 1,
                Name = "User to Delete",
                Email = "delete@example.com",
                Role = "User"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            await _service.DeleteAsync(1);

            userRepositoryMock.Verify(r => r.Remove(user), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepositoryMock.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
        }
    }
}
