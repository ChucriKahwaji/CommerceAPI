using BusinessLogic.Interfaces;
using CommerceEntities.Entities;
using DataAccess.Interfaces;
using log4net;

namespace BusinessLogic.Services.Products
{
    public class ProductService : IProductService
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductService));
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            _logger.Info("Fetching all products from service.");
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            _logger.Info($"Fetching product with ID: {id} from service.");
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            _logger.Info("Adding new product from service.");
            await _productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _logger.Info($"Updating product with ID: {product.Id} from service.");
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            _logger.Info($"Deleting product with ID: {id} from service.");
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
