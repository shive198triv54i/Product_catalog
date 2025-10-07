using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Entities;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.UnitOfWork;
using FluentAssertions;

namespace Test.UnitOfWork
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly ProductCatalogDbContext _context;
        private readonly ProductCatalog.Infrastructure.UnitOfWork.UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            var options = new DbContextOptionsBuilder<ProductCatalogDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductCatalogDbContext(options);
            _unitOfWork = new ProductCatalog.Infrastructure.UnitOfWork.UnitOfWork(_context);
        }

        [Fact]
        public void Constructor_ShouldInitializeAllRepositories()
        {
            _unitOfWork.Users.Should().NotBeNull();
            _unitOfWork.Products.Should().NotBeNull();
            _unitOfWork.Categories.Should().NotBeNull();
        }

        [Fact]
        public async Task CompleteAsync_ShouldSaveChangesToDatabase()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "hashedpassword",
                Role = "User"
            };

            var category = new Category
            {
                Name = "Test Category"
            };

            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = 1
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.Products.AddAsync(product);
            
            var result = await _unitOfWork.CompleteAsync();

            result.Should().BeGreaterThan(0);
            
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            var savedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Test Category");
            var savedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");
            
            savedUser.Should().NotBeNull();
            savedCategory.Should().NotBeNull();
            savedProduct.Should().NotBeNull();
        }

        [Fact]
        public async Task CompleteAsync_WithNoChanges_ShouldReturnZero()
        {
            var result = await _unitOfWork.CompleteAsync();

            result.Should().Be(0);
        }

        [Fact]
        public async Task CompleteAsync_WithMultipleOperations_ShouldSaveAllChanges()
        {
            var user1 = new User { Name = "User 1", Email = "user1@example.com", Password = "pass1", Role = "User" };
            var user2 = new User { Name = "User 2", Email = "user2@example.com", Password = "pass2", Role = "User" };
            var category = new Category { Name = "Test Category" };

            await _unitOfWork.Users.AddAsync(user1);
            await _unitOfWork.Users.AddAsync(user2);
            await _unitOfWork.Categories.AddAsync(category);
            
            var result = await _unitOfWork.CompleteAsync();

            result.Should().Be(3);
            
            var userCount = await _context.Users.CountAsync();
            var categoryCount = await _context.Categories.CountAsync();
            
            userCount.Should().Be(2);
            categoryCount.Should().Be(1);
        }

        [Fact]
        public void Repositories_ShouldBeSameInstanceAcrossMultipleCalls()
        {
            var users1 = _unitOfWork.Users;
            var users2 = _unitOfWork.Users;
            var products1 = _unitOfWork.Products;
            var products2 = _unitOfWork.Products;
            var categories1 = _unitOfWork.Categories;
            var categories2 = _unitOfWork.Categories;

            users1.Should().BeSameAs(users2);
            products1.Should().BeSameAs(products2);
            categories1.Should().BeSameAs(categories2);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
