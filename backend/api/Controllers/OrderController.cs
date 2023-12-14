using Microsoft.AspNetCore.Mvc;
using service.Interfaces;
using infrastructure.Models;
using System.Threading.Tasks;

namespace backend.Controllers;

[ApiController]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [Route("/api/order")]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
    {
        try
        {
            Console.WriteLine($"Creating order for UserId: {order.UserId}");
            var createdOrder = await _orderService.CreateOrder(order);
            return Ok(createdOrder);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpGet]
    [Route("/api/userOrders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllUserOrders()
    {
        try
        {
            var orders = await _orderService.GetAllUserOrders();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpGet]
    [Route("/api/orderDetails")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrderDetails()
    {
        try
        {
            var orders = await _orderService.GetAllOrderDetails();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("/api/order/{id}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        try
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}