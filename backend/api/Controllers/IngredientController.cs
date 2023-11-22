using api.Models;
using Microsoft.AspNetCore.Mvc;
using service;

namespace backend.Controllers;

public class IngredientController : ControllerBase
{
    
    private readonly IIngredientService _service;

    public IngredientController(IIngredientService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [Route("/api/burger/{burgerId}/ingredients")]
    public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredientByBurgerId([FromRoute] int burgerId)
    {
        IEnumerable<Ingredient> ingredients = await _service.GetIngredientByBurgerId(burgerId);

        if (ingredients == null || !ingredients.Any())
        {
            return NotFound($"No ingredients found for burger with ID {burgerId}");
        }

        return Ok(ingredients);
    }
    
    [HttpPost]
    [Route("/api/ingredient")]
    public async Task<ActionResult<Ingredient>> CreateIngredient([FromBody] Ingredient ingredient)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            Ingredient newIngredient = await _service.CreateIngredient(ingredient);
            return CreatedAtAction(nameof(GetIngredientByBurgerId), new { ingredientId = newIngredient.ID }, newIngredient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    
    [HttpDelete]
    [Route("/api/ingredient/{ingredientId}")]
    public async Task<ActionResult> DeleteIngredient([FromRoute] int ingredientId)
    {
        try
        {
            bool isDeleted = await _service.DeleteIngredient(ingredientId);
            if (isDeleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound("Ingredient not found");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    
    [HttpPost]
    [Route("/api/burger/{burgerId}/ingredient/{ingredientId}")]
    public async Task<ActionResult> AddIngredientToBurger([FromRoute] int burgerId, [FromRoute] int ingredientId)
    {
        try
        {
            bool isAdded = await _service.AddIngredientToBurger(burgerId, ingredientId);
            if (isAdded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to add ingredient to burger");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}