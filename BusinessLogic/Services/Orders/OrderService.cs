using BusinessLogic.Interfaces;
using BusinessLogic.Models;
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

        public async Task<IEnumerable<OrderModel>> GetAllOrdersAsync()
        {
            _logger.Info("Fetching all orders from service.");
            var orders = await _orderRepository.GetAllOrdersAsync();

            return orders.Select(o => new OrderModel
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                TotalAmount = o.TotalAmount,
                OrderDate = o.OrderDate
            }).ToList();
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            _logger.Info($"Fetching order with ID: {id} from service.");
            var order = await _orderRepository.GetOrderByIdAsync(id);

            return order == null ? null : new OrderModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate
            };
        }

        public async Task AddOrderAsync(OrderModel orderModel)
        {
            _logger.Info("Adding new order from service.");

            var orderEntity = new Order
            {
                CustomerId = orderModel.CustomerId,
                TotalAmount = orderModel.TotalAmount,
                OrderDate = orderModel.OrderDate
            };

            await _orderRepository.AddOrderAsync(orderEntity);
            orderModel.Id = orderEntity.Id;
        }

        public async Task UpdateOrderAsync(OrderModel orderModel)
        {
            _logger.Info($"Updating order with ID: {orderModel.Id} from service.");

            var orderEntity = new Order
            {
                Id = orderModel.Id,
                CustomerId = orderModel.CustomerId,
                TotalAmount = orderModel.TotalAmount,
                OrderDate = orderModel.OrderDate
            };

            await _orderRepository.UpdateOrderAsync(orderEntity);
        }

        public async Task DeleteOrderAsync(int id)
        {
            _logger.Info($"Deleting order with ID: {id} from service.");
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
