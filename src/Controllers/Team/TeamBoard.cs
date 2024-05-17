using Codraw.Models.Team;
using Codraw.Models.TeamInteraction;
using Codraw.Services.TeamBoardService;
using Codraw.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codraw.Controllers.Team;

[Authorize]
[ApiController]
[Route("api/v0.1")]
public class TeamBoardController(ILogger<TeamBoardController> logger, ITeamBoardClient teamBoardDbService) : ControllerBase
{
    [Authorize]
    [HttpPost("create-teamboard")]
    public async Task<IActionResult> CreateTeamBoard([FromBody] CreateBoard createBoard)
    {
        try
        {
            await teamBoardDbService.CreateTeamBoardAsync(createBoard);
            return Ok("Successfully created team board: " + createBoard.BoardName);
        }
        catch (Exception ex)
        {
            logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }

    [Authorize]
    [HttpGet("teamboard")]
    public async Task<IActionResult> GetTeamBoards([FromQuery] string boardIds)
    {
        try
        {
            var boardIdList = boardIds.StripWhitespace(",");
            if (boardIdList == null || boardIdList.Count == 0)
            {
                return BadRequest("BoardIds cannot be a empty string");
            }
            var teamBoards = await teamBoardDbService.GetTeamBoardsAsync(boardIdList);
            return Ok(teamBoards);
        }
        catch (Exception ex)
        {
            logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }

    [Authorize]
    [HttpPost("update-teamboard")]
    public async Task<IActionResult> UpdateTeamboard([FromBody] TeamBoardUpdate teamBoardUpdate)
    {
        try
        {
            await teamBoardDbService.UpdateTeamBoardsAsync(teamBoardUpdate);
            return Ok("team board updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogInformation("Internal Server Error : {message}", ex.Message);
            return Problem("Something went wrong :(");
        }
    }
}
