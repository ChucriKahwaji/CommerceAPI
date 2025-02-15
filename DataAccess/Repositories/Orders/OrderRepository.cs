using CommerceEntities.Entities;
using DataAccess.Interfaces;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(OrderRepository));
        private readonly CommerceContext _context;

        public OrderRepository(CommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                _logger.Info("Fetching all orders from the database.");
                return await _context.Orders.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while fetching all orders.", ex);
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                _logger.Info($"Fetching order with ID: {id}");
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.Warn($"Order with ID: {id} not found.");
                }
                return order;
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while fetching order with ID: {id}", ex);
                throw;
            }
        }

        public async Task AddOrderAsync(Order order)
        {
            try
            {
                _logger.Info("Adding a new order to the database.");
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                _logger.Info("Order added successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while adding a new order.", ex);
                throw;
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            try
            {
                _logger.Info($"Updating order with ID: {order.Id}");
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                _logger.Info("Order updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while updating order with ID: {order.Id}", ex);
                throw;
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                _logger.Info($"Deleting order with ID: {id}");
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.Warn($"Order with ID: {id} not found.");
                    return;
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                _logger.Info("Order deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while deleting order with ID: {id}", ex);
                throw;
            }
        }
    }
}
