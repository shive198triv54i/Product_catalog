using AutoMapper;
using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Services;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using FluentAssertions;

namespace Test.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _service = new CategoryService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCategorySuccessfully()
        {
            var categoryDto = new CategoryDto
            {
                Name = "Test Category"
            };

            var category = new Category
            {
                Id = 1,
                Name = "Test Category"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            _mapperMock.Setup(m => m.Map<CategoryDto>(category)).Returns(categoryDto);

            var result = await _service.CreateAsync(categoryDto);

            result.Should().NotBeNull();
            result.Name.Should().Be("Test Category");
            categoryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCategory()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Test Category"
            };

            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Test Category"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<CategoryDto>(category)).Returns(categoryDto);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Category");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Category?)null);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<CategoryDto>(null)).Returns((CategoryDto?)null);

            var result = await _service.GetByIdAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };

            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Category 1" },
                new CategoryDto { Id = 2, Name = "Category 2" }
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryDto>>(categories)).Returns(categoryDtos);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(c => c.Name == "Category 1");
            result.Should().Contain(c => c.Name == "Category 2");
        }

        [Fact]
        public async Task UpdateAsync_WithValidCategory_ShouldUpdateSuccessfully()
        {
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Updated Category"
            };

            var existingCategory = new Category
            {
                Id = 1,
                Name = "Original Category"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingCategory);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            await _service.UpdateAsync(categoryDto);

            categoryRepositoryMock.Verify(r => r.Update(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidCategoryId_ShouldThrowKeyNotFoundException()
        {
            var categoryDto = new CategoryDto
            {
                Id = 999,
                Name = "Updated Category"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Category?)null);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(categoryDto));
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteSuccessfully()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Category to Delete"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            await _service.DeleteAsync(1);

            categoryRepositoryMock.Verify(r => r.Remove(category), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Category?)null);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
        }

        [Fact]
        public async Task GetAllAsync_WithEmptyDatabase_ShouldReturnEmptyList()
        {
            var categories = new List<Category>();
            var categoryDtos = new List<CategoryDto>();

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
            _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryDto>>(categories)).Returns(categoryDtos);

            var result = await _service.GetAllAsync();

            result.Should().BeEmpty();
        }
    }
}
