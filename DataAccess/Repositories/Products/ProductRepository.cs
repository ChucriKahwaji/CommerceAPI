using CommerceEntities.Entities;
using DataAccess.Interfaces;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductRepository));
        private readonly CommerceContext _context;

        public ProductRepository(CommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                _logger.Info("Fetching all products from the database.");
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while fetching all products.", ex);
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                _logger.Info($"Fetching product with ID: {id}");
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.Warn($"Product with ID: {id} not found.");
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while fetching product with ID: {id}", ex);
                throw;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                _logger.Info("Adding a new product to the database.");
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                _logger.Info("Product added successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while adding a new product.", ex);
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                _logger.Info($"Updating product with ID: {product.Id}");
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                _logger.Info("Product updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while updating product with ID: {product.Id}", ex);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                _logger.Info($"Deleting product with ID: {id}");
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.Warn($"Product with ID: {id} not found.");
                    return;
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                _logger.Info("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while deleting product with ID: {id}", ex);
                throw;
            }
        }
    }
}
