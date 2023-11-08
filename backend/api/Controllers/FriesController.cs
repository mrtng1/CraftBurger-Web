using System.Diagnostics;
using api.Models; // Assuming Fries model is in this namespace
using Microsoft.AspNetCore.Mvc;
using service; // Assuming IFriesService is in this namespace
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Controllers;

[ApiController]
[Route("/api/fries")]
public class FriesController : Controller
{
    private readonly IFriesService _service;

    public FriesController(IFriesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<Fries>> GetAllFries()
    {
        return await _service.GetAllFries();
    }
    
    [HttpGet("{friesId}")]
    public async Task<ActionResult<Fries>> GetFriesById([FromRoute] int friesId)
    {
        Fries fries = await _service.GetFriesById(friesId);
        if (fries == null)
        {
            return NotFound("Fries not found");
        }
        return Ok(fries);
    }

    [HttpPost]
    public async Task<ActionResult<Fries>> CreateFries([FromBody] Fries fries)
    {
        if (fries == null || string.IsNullOrEmpty(fries.FriesName))
        {
            return BadRequest("Invalid input");
        }

        Fries newFries = await _service.CreateFries(fries);
        return CreatedAtAction(nameof(GetFriesById), new { friesId = newFries.ID }, newFries);
    }

    [HttpPut("{friesId}")]
    public async Task<ActionResult<Fries>> UpdateFries([FromRoute] int friesId, [FromBody] Fries fries)
    {
        if (fries == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid input");
        }

        Fries updatedFries = await _service.UpdateFries(friesId, fries);
        if (updatedFries == null)
        {
            return NotFound("Fries not found");
        }

        return Ok(updatedFries);
    }

    [HttpDelete("{friesId}")]
    public async Task<ActionResult> DeleteFries([FromRoute] int friesId)
    {
        bool isDeleted = await _service.DeleteFries(friesId);
        if (isDeleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound("Fries not found");
        }
    }
}
