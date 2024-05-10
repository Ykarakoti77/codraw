using Codraw.Models.Auth;
using Codraw.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codraw.Controllers;

[Authorize]
[ApiController]
[Route("api/v0.1")]
public class Auth(ILogger<Auth> _logger, IAuthClient _authClient) : ControllerBase
{
    // todo: login should return whole of authtoken (with jwt_token inside)
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Post([FromBody] PasswordLogin loginDetails)
    {
        try
        {
            var token = await _authClient.AuthenticateAsync(loginDetails);
            return Ok(new LoginReponse { BearerToken = token });
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN, TRADER")]
    [HttpGet("logout")]
    public IActionResult Get()
    {
        try
        {
            _logger.LogInformation("Logout was called!");
            return Ok("200, Server No-op!");
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }
}
