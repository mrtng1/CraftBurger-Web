using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using service.Interfaces;
using service.Interfaces.Blob;

namespace backend.Controllers;

[ApiController]
public class FriesController : Controller
{
    private readonly IFriesService _service;
    private readonly IBlobStorageService _blobStorageService;

    public FriesController(IFriesService service, IBlobStorageService blobStorageService)
    {
        _service = service;
        _blobStorageService = blobStorageService;
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
    public async Task<ActionResult<Fries>> CreateFries([FromBody] Fries fries, IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Process the image upload
        if (image != null && image.Length > 0)
        {
            // Save the image to Azure Blob Storage and get the URL
            string imageUrl = await _blobStorageService.UploadFileAsync(image.OpenReadStream(), image.FileName);

            // Set the image URL in the fries model
            fries.ImageUrl = imageUrl;
        }

        // Create the fries
        Fries newFries = await _service.CreateFries(fries);
        return CreatedAtAction(nameof(GetFriesById), new { friesId = newFries.ID }, newFries);
    }


    [Authorize]
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

    [Authorize]
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