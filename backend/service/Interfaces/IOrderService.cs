using infrastructure.Models;

namespace service.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrder(Order order);
}