using infrastructure.Models;

namespace service.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrder(Order order);
    Task<IEnumerable<Order>> GetAllUserOrders();
    Task<IEnumerable<OrderDetail>> GetAllOrderDetails();

    Task<Order> GetOrderById(int id);
}