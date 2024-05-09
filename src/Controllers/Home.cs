using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codraw.Controllers;

[ApiController]
[AllowAnonymous]
[Route("/")]
public class Home(ILogger<Home> _logger) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Health Checked!");
        return Ok("Welcome to CoDraw!");
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        _logger.LogInformation("Pinged!");
        return Ok("Pong");
    }
}
