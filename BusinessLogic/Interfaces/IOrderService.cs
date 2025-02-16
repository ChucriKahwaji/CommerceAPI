using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel> GetOrderByIdAsync(int id);
        Task AddOrderAsync(OrderModel order);
        Task UpdateOrderAsync(OrderModel order);
        Task DeleteOrderAsync(int id);
    }
}
