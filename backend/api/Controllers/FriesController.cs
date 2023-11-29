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

        if (image != null)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string imageUrl = await _blobStorageService.UploadFileAsync(image.OpenReadStream(), uniqueFileName);
            fries.ImageUrl = imageUrl;
        }

        Fries newFries = await _service.CreateFries(fries);
        return CreatedAtAction(nameof(GetFriesById), new { friesId = newFries.ID }, newFries);
    }

    [HttpPut]
    [Route("/api/fries/{friesId}")]
    public async Task<ActionResult<Fries>> UpdateFries([FromRoute] int friesId, [FromBody] Fries fries, IFormFile image)
    {
        if (!ModelState.IsValid || fries.ID != friesId)
        {
            return BadRequest(ModelState);
        }

        if (image != null)
        {
            if (!string.IsNullOrEmpty(fries.ImageUrl))
            {
                Uri uri = new Uri(fries.ImageUrl);
                string fileName = Path.GetFileName(uri.LocalPath);
                await _blobStorageService.DeleteFileAsync(fileName);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string imageUrl = await _blobStorageService.UploadFileAsync(image.OpenReadStream(), uniqueFileName);
            fries.ImageUrl = imageUrl;
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