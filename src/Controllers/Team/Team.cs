using System.Security.Claims;
using Codraw.Models.TeamInteraction;
using Codraw.Services.TeamsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codraw.Controllers.Team;

[Authorize]
[ApiController]
[Route("api/v0.1")]
public class TeamsController(ILogger<TeamsController> logger, ITeamsClient teamsClient)
    : ControllerBase
{
    [Authorize]
    [HttpGet("teams")]
    public async Task<IActionResult> GetTeams()
    {
        try
        {
            var userId =
                User.FindFirstValue("UserId") ?? throw new Exception("Internal server error");
            var teams = await teamsClient.GetTeamsByUserIdAsync(userId);

            return Ok(teams);
        }
        catch (Exception ex)
        {
            logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }

    [Authorize]
    [HttpPost("create-team")]
    public async Task<IActionResult> CreateTeam([FromQuery] string teamName)
    {
        try
        {
            var userId =
                User.FindFirstValue("UserId") ?? throw new Exception("Internal server error");
            await teamsClient.CreateTeamAsync(teamName, userId);

            return Ok("Successfully created team: " + teamName);
        }
        catch (Exception ex)
        {
            logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }

    [Authorize]
    [HttpPost("join-team")]
    public async Task<IActionResult> JoinTeam([FromQuery] string teamInviteId)
    {
        try
        {
            var userId =
                User.FindFirstValue("UserId") ?? throw new Exception("Internal server error");
            await teamsClient.JoinTeamAsync(teamInviteId, userId);

            return Ok("Successfully Joined team: " + teamInviteId);
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Internal Server Error : {ex.Message}");
            return Problem("Something went wrong :(");
        }
    }

    [Authorize]
    [HttpPost("leave-team")]
    public async Task<IActionResult> LeaveTeam([FromQuery] string teamId)
    {
        try
        {
            var userId =
                User.FindFirstValue("UserId") ?? throw new Exception("Internal server error");
            var removeInfo = new RemoveTeamMember () {
                UserId = userId,
                RemoverId = userId,
                TeamId = teamId
            };
            await teamsClient.RemoveTeamMemberAsync(removeInfo);

            return Ok("Successfully Left team: " + teamId);
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Internal Server Error : {ex.Message}");
            return Problem("Something went wrong :(");
        }
    }

    [Authorize]
    [HttpPost("remove-teammember")]
    public async Task<IActionResult> RemoveTeamMember(
        [FromBody] RemoveTeamMember removeInfo
    )
    {
        try
        {
            await teamsClient.RemoveTeamMemberAsync(removeInfo);
            return Ok("Successfully Removed team member");
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Internal Server Error : {ex.Message}");
            return Problem("Something went wrong :(");
        }
    }
}
