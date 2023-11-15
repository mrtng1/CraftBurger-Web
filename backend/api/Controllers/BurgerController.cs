using System.Diagnostics;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

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

    [Authorize]
    [HttpPost]
    [Route("/api/burger")]
    public async Task<ActionResult<Burger>> CreateBurger([FromBody] Burger burger)
    {
        if (!ModelState.IsValid)
        {
            
            return BadRequest(ModelState);
        }

        Burger newBurger = await _service.CreateBurger(burger);
        return CreatedAtAction(nameof(GetBurgerById), new { burgerId = newBurger.ID }, newBurger);
    }

    [Authorize]
    [HttpPut]
    [Route("/api/burger/{burgerId}")]
    public async Task<ActionResult<Burger>> UpdateBurger([FromBody] Burger burger, [FromRoute] int burgerId)
    {
        if (!ModelState.IsValid || burger.ID != burgerId)
        {
            return BadRequest(ModelState);
        }

        Burger updatedBurger = await _service.UpdateBurger(burgerId, burger);

        if (updatedBurger == null)
        {
            return NotFound("Burger not found");
        }

        return Ok(updatedBurger);
    }

    [Authorize]
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
    
    [HttpGet]
    [Route("/api/burger/{burgerId}/ingredients")]
    public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredientsByBurgerId([FromRoute] int burgerId)
    {
        IEnumerable<Ingredient> ingredients = await _service.GetIngredientsByBurgerId(burgerId);

        if (ingredients == null || !ingredients.Any())
        {
            return NotFound($"No ingredients found for burger with ID {burgerId}");
        }

        return Ok(ingredients);
    }
}
