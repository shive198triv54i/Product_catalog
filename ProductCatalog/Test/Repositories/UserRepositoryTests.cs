using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Entities;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;
using FluentAssertions;

namespace Test.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly ProductCatalogDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ProductCatalogDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductCatalogDbContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "hashedpassword",
                Role = "User"
            };

            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            savedUser.Should().NotBeNull();
            savedUser!.Name.Should().Be("Test User");
            savedUser.Email.Should().Be("test@example.com");
            savedUser.Password.Should().Be("hashedpassword");
            savedUser.Role.Should().Be("User");
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnUser()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "hashedpassword",
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(user.Id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test User");
            result.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var result = await _repository.GetByIdAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ShouldReturnUser()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "hashedpassword",
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByEmailAsync("test@example.com");

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test User");
            result.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task GetByEmailAsync_WithInvalidEmail_ShouldReturnNull()
        {
            var result = await _repository.GetByEmailAsync("nonexistent@example.com");

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var users = new List<User>
            {
                new User { Name = "User 1", Email = "user1@example.com", Password = "pass1", Role = "User" },
                new User { Name = "User 2", Email = "user2@example.com", Password = "pass2", Role = "Admin" },
                new User { Name = "User 3", Email = "user3@example.com", Password = "pass3", Role = "User" }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            result.Should().HaveCount(3);
            result.Should().Contain(u => u.Email == "user1@example.com");
            result.Should().Contain(u => u.Email == "user2@example.com");
            result.Should().Contain(u => u.Email == "user3@example.com");
        }

        [Fact]
        public void Update_ShouldUpdateUserInDatabase()
        {
            var user = new User
            {
                Name = "Original Name",
                Email = "original@example.com",
                Password = "originalpass",
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            user.Name = "Updated Name";
            user.Email = "updated@example.com";
            user.Role = "Admin";
            _repository.Update(user);
            _context.SaveChanges();

            var updatedUser = _context.Users.Find(user.Id);
            updatedUser!.Name.Should().Be("Updated Name");
            updatedUser.Email.Should().Be("updated@example.com");
            updatedUser.Role.Should().Be("Admin");
        }

        [Fact]
        public void Remove_ShouldRemoveUserFromDatabase()
        {
            var user = new User
            {
                Name = "User to Remove",
                Email = "remove@example.com",
                Password = "password",
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            _repository.Remove(user);
            _context.SaveChanges();

            var removedUser = _context.Users.Find(user.Id);
            removedUser.Should().BeNull();
        }

        [Fact]
        public async Task GetByEmailAsync_WithCaseInsensitiveEmail_ShouldReturnUser()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "hashedpassword",
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByEmailAsync("TEST@EXAMPLE.COM");

            result.Should().NotBeNull();
            result!.Email.Should().Be("test@example.com");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
