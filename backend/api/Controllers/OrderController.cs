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
}