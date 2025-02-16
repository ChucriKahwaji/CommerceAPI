using BusinessLogic.Interfaces;
using BusinessLogic.Models;
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

        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            _logger.Info("Fetching all products from service.");
            var products = await _productRepository.GetAllProductsAsync();

            // Convert Entities → Business Models
            return products.Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            _logger.Info($"Fetching product with ID: {id} from service.");
            var product = await _productRepository.GetProductByIdAsync(id);

            return product == null ? null : new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task AddProductAsync(ProductModel productModel)
        {
            _logger.Info("Adding new product from service.");

            // Convert Business Model → Entity
            var productEntity = new Product
            {
                Name = productModel.Name,
                Price = productModel.Price,
                CreatedAt = productModel.CreatedAt
            };

            await _productRepository.AddProductAsync(productEntity);
            productModel.Id = productEntity.Id;
        }

        public async Task UpdateProductAsync(ProductModel productModel)
        {
            _logger.Info($"Updating product with ID: {productModel.Id} from service.");

            // Convert Business Model → Entity
            var productEntity = new Product
            {
                Id = productModel.Id,
                Name = productModel.Name,
                Price = productModel.Price,
                CreatedAt = productModel.CreatedAt
            };

            await _productRepository.UpdateProductAsync(productEntity);
        }

        public async Task DeleteProductAsync(int id)
        {
            _logger.Info($"Deleting product with ID: {id} from service.");
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
