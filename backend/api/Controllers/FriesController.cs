using System.Diagnostics;
using api.Models; // Assuming Fries model is in this namespace
using Microsoft.AspNetCore.Mvc;
using service; // Assuming IFriesService is in this namespace
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Controllers;

[ApiController]
public class FriesController : Controller
{
    private readonly IFriesService _service;

    public FriesController(IFriesService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("/api/fries")]
    public async Task<IEnumerable<Fries>> GetAllFries()
    {
        return await _service.GetAllFries();
    }
    
    [HttpGet]
    [Route("/api/fries/{friesId}")]
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
    [Route("/api/fries")]
    public async Task<ActionResult<Fries>> CreateFries([FromBody] Fries fries)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Fries newFries = await _service.CreateFries(fries);
        return CreatedAtAction(nameof(GetFriesById), new { friesId = newFries.ID }, newFries);
    }
    
    [HttpPut]
    [Route("/api/fries/{friesId}")]
    public async Task<ActionResult<Fries>> UpdateFries([FromRoute] int friesId, [FromBody] Fries fries)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (fries.ID != friesId)
        {
            return BadRequest("Mismatch between the ID in the route and the body.");
        }

        Fries updatedFries = await _service.UpdateFries(friesId, fries);
        if (updatedFries == null)
        {
            return NotFound("Fries not found");
        }

        return Ok(updatedFries);
    }
    
    [HttpDelete]
    [Route("/api/fries/{friesId}")]
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
