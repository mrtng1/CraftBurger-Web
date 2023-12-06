using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using service;

namespace backend.Controllers;

public class MailController : ControllerBase
{
    private readonly MailService _mailService;

    public MailController(MailService mailService)
    {
        _mailService = mailService;
    }
    
    [HttpPost("SendMail")]
    public IActionResult SendMail([FromBody] MailRequest request)
    {
        try
        {
            _mailService.sendMail(request.To, request.Subject, request.Body);
            return Ok(new { message = "Email sent successfully" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}