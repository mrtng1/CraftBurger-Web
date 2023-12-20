using System;
using System.Threading.Tasks;
using infrastructure.Models;
using infrastructure.Repositories;
using service.Interfaces;

namespace service.Services;

public class OrderService : IOrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly UserRepository _userRepository;

    public OrderService(OrderRepository orderRepository, UserRepository userRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    }

    public async Task<Order> CreateOrder(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        bool userExists = await _userRepository.UserExists(order.UserId);
        if (!userExists)
        {
            throw new ArgumentException("Invalid UserId: User does not exist.");
        }

        var createdOrder = await _orderRepository.CreateOrder(order);
        
        foreach (var detail in order.OrderDetails)
        {
            detail.OrderId = createdOrder.OrderId; 
            await _orderRepository.CreateOrderDetail(detail);
        }

        return createdOrder;
    }
    
    public async Task<IEnumerable<Order>> GetAllUserOrders()
    {
        return await _orderRepository.GetAllUserOrders();
    }
    
    public async Task<IEnumerable<OrderDetail>> GetAllOrderDetails()
    {
        return await _orderRepository.GetAllOrderDetails();
    }

    public async Task<Order> GetOrderById(int id)
    {
        return await _orderRepository.GetOrderById(id);
    }
}