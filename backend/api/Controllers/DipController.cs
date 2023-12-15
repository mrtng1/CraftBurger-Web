using System.Diagnostics;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Controllers;

public class DipController : Controller
{
    private readonly IDipService _service;

    public DipController(IDipService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("/api/dips")]
    public async Task<IEnumerable<Dip>> GetDips()
    {
        return await _service.GetAllDips();
    }

    [HttpGet]
    [Route("/api/dip/{dipId}")]
    public async Task<ActionResult<Dip>> GetDipById([FromRoute] int dipId)
    {
        Dip dip = await _service.GetDipById(dipId);
        if (dip == null)
        {
            return NotFound("Dip not found");
        }
        return Ok(dip);
    }

    [HttpPost]
    [Route("/api/dip")]
    public async Task<ActionResult<Dip>> CreateDip([FromBody] Dip dip)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Dip newDip = await _service.CreateDip(dip);
        return CreatedAtAction(nameof(GetDipById), new { dipId = newDip.ID }, newDip);
    }

    [HttpPut]
    [Route("/api/dip/{dipId}")]
    public async Task<ActionResult<Dip>> UpdateDip([FromBody] Dip dip, [FromRoute] int dipId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Dip updatedDip = await _service.UpdateDip(dipId, dip);
        if (updatedDip == null)
        {
            return NotFound("Dip not found");
        }

        return Ok(updatedDip);
    }

    //[Authorize]
    [HttpDelete]
    [Route("/api/dip/{dipId}")]
    public async Task<ActionResult> DeleteDip([FromRoute] int dipId)
    {
        bool isDeleted = await _service.DeleteDip(dipId);
        if (isDeleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound("Dip not found");
        }
    }
}
