using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Entities;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;
using FluentAssertions;

namespace Test.Repositories
{
    public class CategoryRepositoryTests : IDisposable
    {
        private readonly ProductCatalogDbContext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ProductCatalogDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductCatalogDbContext(options);
            _repository = new CategoryRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "Test Category"
            };

            // Act
            await _repository.AddAsync(category);
            await _context.SaveChangesAsync();

            // Assert
            var savedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Test Category");
            savedCategory.Should().NotBeNull();
            savedCategory!.Name.Should().Be("Test Category");
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCategory()
        {
            // Arrange
            var category = new Category
            {
                Name = "Test Category"
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(category.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Category");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Name = "Category 1" },
                new Category { Name = "Category 2" },
                new Category { Name = "Category 3" }
            };

            _context.Categories.AddRange(categories);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Contain(c => c.Name == "Category 1");
            result.Should().Contain(c => c.Name == "Category 2");
            result.Should().Contain(c => c.Name == "Category 3");
        }

        [Fact]
        public void Update_ShouldUpdateCategoryInDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "Original Name"
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            // Act
            category.Name = "Updated Name";
            _repository.Update(category);
            _context.SaveChanges();

            // Assert
            var updatedCategory = _context.Categories.Find(category.Id);
            updatedCategory!.Name.Should().Be("Updated Name");
        }

        [Fact]
        public void Remove_ShouldRemoveCategoryFromDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "Category to Remove"
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            // Act
            _repository.Remove(category);
            _context.SaveChanges();

            // Assert
            var removedCategory = _context.Categories.Find(category.Id);
            removedCategory.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_WithEmptyDatabase_ShouldReturnEmptyList()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
