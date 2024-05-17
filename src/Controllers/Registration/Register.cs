using Codraw.Models.Registration;
using Codraw.Models.User;
using Codraw.Services.RegistrationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codraw.Controllers.Auth;

[Authorize]
[ApiController]
[Route("api/v0.1")]
public class RegistrationController(ILogger<Auth> logger, IRegistrationClient registrationClient) : ControllerBase
{
    // todo: login should return whole of authtoken (with jwt_token inside)
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDetails registrationInfo)
    {
        try
        {
            var userDetails = new UserDetails() {
                Email = registrationInfo.Email,
                Password = registrationInfo.Password,
                UserName = registrationInfo.UserName,
                FirstName = registrationInfo.FirstName,
                LastName = registrationInfo.LastName
            };
            await registrationClient.RegisterUserAsync(userDetails);
            return Ok("Successfully registered!");
        }
        catch (Exception ex)
        {
            logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }
}
