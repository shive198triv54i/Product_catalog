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
            var category = new Category
            {
                Name = "Test Category"
            };

            await _repository.AddAsync(category);
            await _context.SaveChangesAsync();

            var savedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Test Category");
            savedCategory.Should().NotBeNull();
            savedCategory!.Name.Should().Be("Test Category");
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCategory()
        {
            var category = new Category
            {
                Name = "Test Category"
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(category.Id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Category");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var result = await _repository.GetByIdAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            var categories = new List<Category>
            {
                new Category { Name = "Category 1" },
                new Category { Name = "Category 2" },
                new Category { Name = "Category 3" }
            };

            _context.Categories.AddRange(categories);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            result.Should().HaveCount(3);
            result.Should().Contain(c => c.Name == "Category 1");
            result.Should().Contain(c => c.Name == "Category 2");
            result.Should().Contain(c => c.Name == "Category 3");
        }

        [Fact]
        public void Update_ShouldUpdateCategoryInDatabase()
        {
            var category = new Category
            {
                Name = "Original Name"
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            category.Name = "Updated Name";
            _repository.Update(category);
            _context.SaveChanges();

            var updatedCategory = _context.Categories.Find(category.Id);
            updatedCategory!.Name.Should().Be("Updated Name");
        }

        [Fact]
        public void Remove_ShouldRemoveCategoryFromDatabase()
        {
            var category = new Category
            {
                Name = "Category to Remove"
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            _repository.Remove(category);
            _context.SaveChanges();

            var removedCategory = _context.Categories.Find(category.Id);
            removedCategory.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_WithEmptyDatabase_ShouldReturnEmptyList()
        {
            var result = await _repository.GetAllAsync();

            result.Should().BeEmpty();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
