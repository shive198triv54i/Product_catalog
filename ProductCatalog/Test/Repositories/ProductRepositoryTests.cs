using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Entities;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;
using FluentAssertions;

namespace Test.Repositories
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ProductCatalogDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ProductCatalogDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductCatalogDbContext(options);
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = 1
            };

            // Act
            await _repository.AddAsync(product);
            await _context.SaveChangesAsync();

            // Assert
            var savedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");
            savedProduct.Should().NotBeNull();
            savedProduct!.Name.Should().Be("Test Product");
            savedProduct.Description.Should().Be("Test Description");
            savedProduct.Price.Should().Be(99.99m);
            savedProduct.CategoryId.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = 1
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(product.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Product");
            result.Description.Should().Be("Test Description");
            result.Price.Should().Be(99.99m);
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
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Name = "Product 1", Price = 10.00m, CategoryId = 1 },
                new Product { Name = "Product 2", Price = 20.00m, CategoryId = 2 },
                new Product { Name = "Product 3", Price = 30.00m, CategoryId = 1 }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Contain(p => p.Name == "Product 1");
            result.Should().Contain(p => p.Name == "Product 2");
            result.Should().Contain(p => p.Name == "Product 3");
        }

        [Fact]
        public async Task GetProductsWithCategoryAsync_ShouldReturnProductsWithCategory()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var product = new Product
            {
                Name = "Test Product",
                Price = 99.99m,
                CategoryId = category.Id,
                Category = category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetProductsWithCategoryAsync();

            // Assert
            result.Should().HaveCount(1);
            var firstProduct = result.First();
            firstProduct.Name.Should().Be("Test Product");
            firstProduct.Category.Should().NotBeNull();
            firstProduct.Category!.Name.Should().Be("Test Category");
        }

        [Fact]
        public void Update_ShouldUpdateProductInDatabase()
        {
            // Arrange
            var product = new Product
            {
                Name = "Original Name",
                Price = 50.00m,
                CategoryId = 1
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            product.Name = "Updated Name";
            product.Price = 75.00m;
            _repository.Update(product);
            _context.SaveChanges();

            // Assert
            var updatedProduct = _context.Products.Find(product.Id);
            updatedProduct!.Name.Should().Be("Updated Name");
            updatedProduct.Price.Should().Be(75.00m);
        }

        [Fact]
        public void Remove_ShouldRemoveProductFromDatabase()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product to Remove",
                Price = 25.00m,
                CategoryId = 1
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            _repository.Remove(product);
            _context.SaveChanges();

            // Assert
            var removedProduct = _context.Products.Find(product.Id);
            removedProduct.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
