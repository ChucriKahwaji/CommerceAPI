using BusinessLogic.Interfaces;
using CommerceEntities.Entities;
using DataAccess.Interfaces;
using log4net;

namespace BusinessLogic.Services.Orders
{
    public class OrderService : IOrderService
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(OrderService));
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            _logger.Info("Fetching all orders from service.");
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            _logger.Info($"Fetching order with ID: {id} from service.");
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task AddOrderAsync(Order order)
        {
            _logger.Info("Adding new order from service.");
            await _orderRepository.AddOrderAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _logger.Info($"Updating order with ID: {order.Id} from service.");
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            _logger.Info($"Deleting order with ID: {id} from service.");
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
