using AutoMapper;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetProductsWithCategoryAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productDto.Id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            _mapper.Map(productDto, product);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();
        }
    }
}
