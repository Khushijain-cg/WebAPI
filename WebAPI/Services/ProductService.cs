using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTO;
using WebAPI.Models;
namespace WebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;
        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    Stock = p.Stock
                })
                .ToListAsync();
        }
        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock
            };
        }
        public async Task<ProductDto> CreateProductAsync(ProductDto dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Stock = dto.Stock,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                UpdatedDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Product Created. ProductId: {ProductId}, Name: {ProductName}",
                product.Id,
                product.Name);

            dto.Id = product.Id;
            return dto;
        }
        [HttpPut("{id}")]
        public async Task<bool> UpdateAsync(Guid id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            if (dto.Name != null)
                product.Name = dto.Name;

            if (dto.Category != null)
                product.Category = dto.Category;

            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;

            if (dto.Stock.HasValue)
                product.Stock = dto.Stock.Value;

            product.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Product Updated. ProductId: {ProductId}",
                product.Id);


            return true;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Product Deleted. ProductId: {ProductId}",
                product.Id);

            return true;
        }
        public async Task<IEnumerable<ProductDto>> SearchByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category.ToLower().Contains(category.ToLower()))
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    Stock = p.Stock
                })
                .ToListAsync();
        }
    }
}