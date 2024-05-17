using Codraw.Models.Auth;
using Codraw.Services.AuthService;
using Codraw.Services.GoogleAuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codraw.Controllers.Auth;

[Authorize]
[ApiController]
[Route("api/v0.1")]
public class Auth(ILogger<Auth> logger, IAuthClient authClient, IGoogleAuthClient googleAuthClient) : ControllerBase
{
    // todo: login should return whole of authtoken (with jwt_token inside)
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] PasswordLogin loginDetails)
    {
        try
        {
            var token = await authClient.AuthenticateAsync(loginDetails);
            return Ok(new LoginReponse { BearerToken = token });
        }
        catch (Exception ex)
        {
            logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }

    [AllowAnonymous]
    [HttpPost("glogin")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLogin loginDetails)
    {
        try
        {
            var token = await googleAuthClient.GoogleSignInAsync(loginDetails);
            return Ok(new LoginReponse { BearerToken = token });
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Internal Server Error : {ex.Message}");
            return Problem("Something went wrong :(");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN, TRADER")]
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        try
        {
            logger.LogInformation("Logout was called!");
            return Ok("200, Server No-op!");
        }
        catch (Exception ex)
        {
            logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }
}
