using System.Diagnostics;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using service;
using System.Threading.Tasks;

namespace backend.Controllers;

public class BurgerController : Controller
{
    private readonly IBurgerService _service;

    public BurgerController(IBurgerService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("/api/burgers")]
    public async Task<IEnumerable<Burger>> GetBurgers()
    {
        return await _service.GetAllBurgers();
    }
    
    [HttpGet]
    [Route("/api/burger/{burgerId}")]
    public async Task<ActionResult<Burger>> GetBurgerById([FromRoute] int burgerId)
    {
        Burger burger = await _service.GetBurgerById(burgerId);
        if (burger == null)
        {
            return NotFound("Burger not found");
        }
        return Ok(burger);
    }

    [HttpPost]
    [Route("/api/burger")]
    public async Task<ActionResult<Burger>> PostBurger([FromBody] Burger burger)
    {
        if (burger == null || string.IsNullOrEmpty(burger.BurgerName))
        {
            return BadRequest("Invalid input");
        }

        Burger newBurger = await _service.CreateBurger(burger);
        return Ok(newBurger);
    }

    [HttpPut]
    [Route("/api/burger/{burgerId}")]
    public async Task<ActionResult<Burger>> UpdateBurger([FromBody] Burger burger, [FromRoute] int burgerId)
    {
        if (burger == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid input");
        }

        Burger updatedBurger = await _service.UpdateBurger(burgerId, burger);

        if (updatedBurger == null)
        {
            return NotFound("Burger not found");
        }

        return Ok(updatedBurger);
    }

    [HttpDelete]
    [Route("/api/burger/{burgerId}")]
    public async Task<ActionResult> DeleteBurger([FromRoute] int burgerId)
    {
        bool isDeleted = await _service.DeleteBurger(burgerId);
        if (isDeleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound("Burger not found");
        }
    }
}
