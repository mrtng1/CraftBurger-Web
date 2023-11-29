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
    public async Task<ActionResult<Burger>> CreateBurger([FromForm] Burger burger, [FromForm] IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (image != null)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string imageUrl = await _blobStorageService.UploadFileAsync(image.OpenReadStream(), uniqueFileName);
            burger.ImageUrl = imageUrl;
        }

        Burger newBurger = await _service.CreateBurger(burger);
        return CreatedAtAction(nameof(GetBurgerById), new { burgerId = newBurger.ID }, newBurger);
    }

    [HttpPut]
    [Route("/api/burger/{burgerId}")]
    public async Task<ActionResult<Burger>> UpdateBurger([FromRoute] int burgerId, [FromForm] Burger burger, IFormFile image)
    {
        if (!ModelState.IsValid || burger.ID != burgerId)
        {
            return BadRequest(ModelState);
        }

        if (image != null)
        {
            if (!string.IsNullOrEmpty(burger.ImageUrl))
            {
                Uri uri = new Uri(burger.ImageUrl);
                string fileName = Path.GetFileName(uri.LocalPath);
                await _blobStorageService.DeleteFileAsync(fileName);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string imageUrl = await _blobStorageService.UploadFileAsync(image.OpenReadStream(), uniqueFileName);
            burger.ImageUrl = imageUrl;
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