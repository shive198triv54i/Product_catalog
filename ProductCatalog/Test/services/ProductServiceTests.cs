using AutoMapper;
using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using FluentAssertions;

namespace Test.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _service = new ProductService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProductSuccessfully()
        {
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = 1
            };

            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = 1
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<Product>(productDto)).Returns(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            var result = await _service.CreateAsync(productDto);

            result.Should().NotBeNull();
            result.Name.Should().Be("Test Product");
            result.Price.Should().Be(99.99m);
            productRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = 1
            };

            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = 1
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Product");
            result.Price.Should().Be(99.99m);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<ProductDto>(null)).Returns((ProductDto?)null);

            var result = await _service.GetByIdAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.00m, CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20.00m, CategoryId = 2 }
            };

            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Price = 10.00m, CategoryId = 1 },
                new ProductDto { Id = 2, Name = "Product 2", Price = 20.00m, CategoryId = 2 }
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(r => r.GetProductsWithCategoryAsync()).ReturnsAsync(products);
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(p => p.Name == "Product 1");
            result.Should().Contain(p => p.Name == "Product 2");
        }

        [Fact]
        public async Task UpdateAsync_WithValidProduct_ShouldUpdateSuccessfully()
        {
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 149.99m,
                CategoryId = 1
            };

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Original Product",
                Description = "Original Description",
                Price = 99.99m,
                CategoryId = 1
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingProduct);
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            await _service.UpdateAsync(productDto);

            productRepositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidProductId_ShouldThrowKeyNotFoundException()
        {
            var productDto = new ProductDto
            {
                Id = 999,
                Name = "Updated Product",
                Price = 149.99m,
                CategoryId = 1
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepositoryMock.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(productDto));
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteSuccessfully()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Product to Delete",
                Price = 99.99m,
                CategoryId = 1
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            await _service.DeleteAsync(1);

            productRepositoryMock.Verify(r => r.Remove(product), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepositoryMock.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
        }
    }
}
