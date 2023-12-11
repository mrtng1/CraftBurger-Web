using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;
using service.Interfaces.Blob;

namespace backend.Controllers;

[ApiController]
public class BurgerController : Controller
{
    private readonly IBurgerService _service;
    private readonly IBlobStorageService _blobStorageService;

    public BurgerController(IBurgerService service, IBlobStorageService blobStorageService)
    {
        _service = service;
        _blobStorageService = blobStorageService;
    }
    
    [HttpGet]
    [Route("/api/burgers")]
    public async Task<IEnumerable<Burger>> GetBurgers()
    {
        return await _service.GetAllBurgers();
    }

    [HttpGet]
    [Route("/api/burger/{id}")]
    public async Task<ActionResult<Burger>> GetBurgerById([FromRoute] int id)
    {
        Burger burger = await _service.GetBurgerById(id);
        if (burger == null)
        {
            return NotFound("Burger not found");
        }
        return Ok(burger);
    }

    [HttpPost]
    [Route("/api/burger")]
    public async Task<ActionResult<Burger>> CreateBurger([FromForm] Burger burger, [FromForm] IFormFile image)
    {
        try
        {
            if (image != null)
            {
                // Upload the image file to Azure Blob Storage and get the URL
                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string imageUrl = await _blobStorageService.UploadFileAsync("burgers", image.OpenReadStream(), uniqueFileName);
                burger.imageUrl = imageUrl; // Set the imageUrl in the burger object
            }
            
            var createdBurger = await _service.CreateBurger(burger);
            return Ok(createdBurger);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error: Could not create the burger");
        }
    }

    
    [HttpPut]
    [Route("/api/burger/{burgerId}")]
    public async Task<ActionResult<Burger>> UpdateBurger([FromRoute] int burgerId, [FromForm] Burger burger, [FromForm] IFormFile? image)
    {
        if (!ModelState.IsValid || burger.id != burgerId)
        {
            var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            // Log the errors for debugging
            Console.WriteLine("ModelState Errors: " + string.Join(", ", modelStateErrors));

            return BadRequest(ModelState);
        }

        try
        {
            // Retrieve the current burger data from the database
            Burger currentBurger = await _service.GetBurgerById(burgerId);
            if (currentBurger == null)
            {
                return NotFound("Burger not found");
            }

            // Check if a new image is provided
            if (image != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(currentBurger.imageUrl))
                {
                    Uri uri = new Uri(currentBurger.imageUrl);
                    string fileName = Path.GetFileName(uri.LocalPath);
                    await _blobStorageService.DeleteFileAsync("burgers", fileName);
                }

                // Upload the new image and update the burger object
                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string newImageUrl = await _blobStorageService.UploadFileAsync("burgers", image.OpenReadStream(), uniqueFileName);
                burger.imageUrl = newImageUrl;
            }
            else
            {
                // If no new image is provided, retain the existing imageUrl
                burger.imageUrl = currentBurger.imageUrl;
            }

            // Update the burger in the database
            Burger updatedBurger = await _service.UpdateBurger(burgerId, burger);
            if (updatedBurger == null)
            {
                return NotFound("Burger not found");
            }

            return Ok(updatedBurger);
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpDelete]
    [Route("/api/burger/{id}")]
    public async Task<ActionResult> DeleteBurger([FromRoute] int id)
    {
        // Retrieve the burger item to get the image URL
        Burger burger = await _service.GetBurgerById(id);
        if (burger == null)
        {
            return NotFound("Burger not found");
        }

        // Delete the image from Azure Blob Storage if an imageUrl exists
        if (!string.IsNullOrEmpty(burger.imageUrl))
        {
            Uri uri = new Uri(burger.imageUrl);
            string fileName = Path.GetFileName(uri.LocalPath);
            await _blobStorageService.DeleteFileAsync("burgers", fileName);
        }

        // Delete the burger item from the database
        bool isDeleted = await _service.DeleteBurger(id);
        if (isDeleted)
        {
            return Ok(isDeleted);
        }
        else
        {
            return StatusCode(500, "Error deleting the burger");
        }
    }
}