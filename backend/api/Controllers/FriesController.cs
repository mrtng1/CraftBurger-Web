using infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("/api/fries/{id}")]
    public async Task<ActionResult<Fries>> GetFriesById([FromRoute] int id)
    {
        Fries fries = await _service.GetFriesById(id);
        if (fries == null)
        {
            return NotFound("Fries not found");
        }
        return Ok(fries);
    }

    [HttpPost]
    [Route("/api/fries")]
    [Authorize(Roles = "true")]
    public async Task<ActionResult<Fries>> CreateFries([FromForm] Fries fries, [FromForm] IFormFile image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (image != null)
            {
                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string imageUrl = await _blobStorageService.UploadFileAsync("fries", image.OpenReadStream(), uniqueFileName);
                fries.imageUrl = imageUrl;
            }

            Fries newFries = await _service.CreateFries(fries);
            return Ok(newFries);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error: Could not create the fries");
        }
    }

    [HttpPut]
    [Route("/api/fries/{id}")]
    [Authorize(Roles = "true")]
    public async Task<ActionResult<Fries>> UpdateFries([FromRoute] int id, [FromForm] Fries fries, [FromForm] IFormFile image)
    {
        if (!ModelState.IsValid || fries.id != id)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Retrieve the current fries data from the database
            Fries currentFries = await _service.GetFriesById(id);
            if (currentFries == null)
            {
                return NotFound("Fries not found");
            }

            // Check if a new image is provided
            if (image != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(currentFries.imageUrl))
                {
                    Uri uri = new Uri(currentFries.imageUrl);
                    string fileName = Path.GetFileName(uri.LocalPath);
                    await _blobStorageService.DeleteFileAsync("fries", fileName);
                }

                // Upload the new image and update the fries object
                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string newImageUrl = await _blobStorageService.UploadFileAsync("fries", image.OpenReadStream(), uniqueFileName);
                fries.imageUrl = newImageUrl;
            }
            else
            {
                // If no new image is provided, retain the existing imageUrl
                fries.imageUrl = currentFries.imageUrl;
            }

            // Update the fries in the database
            Fries updatedFries = await _service.UpdateFries(id, fries);
            if (updatedFries == null)
            {
                return NotFound("Fries not found");
            }

            return Ok(updatedFries);
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpDelete]
    [Route("/api/fries/{id}")]
    [Authorize(Roles = "true")]
    public async Task<ActionResult> DeleteFries([FromRoute] int id)
    {
        Fries fries = await _service.GetFriesById(id);
        if (fries == null)
        {
            return NotFound("Fries not found");
        }

        if (!string.IsNullOrEmpty(fries.imageUrl))
        {
            Uri uri = new Uri(fries.imageUrl);
            string fileName = Path.GetFileName(uri.LocalPath);
            await _blobStorageService.DeleteFileAsync("fries", fileName);
        }

        bool isDeleted = await _service.DeleteFries(id);
        if (isDeleted)
        {
            return Ok(isDeleted);
        }
        else
        {
            return StatusCode(500, "Error deleting the fries");
        }
    }
}