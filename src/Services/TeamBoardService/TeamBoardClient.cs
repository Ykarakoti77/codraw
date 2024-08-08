using Codraw.Framework.DbCore;
using Codraw.Models.Team;
using Codraw.Models.TeamInteraction;
using Microsoft.IdentityModel.Tokens;

namespace Codraw.Services.TeamBoardService;

public class TeamBoardClient(
    ILogger<TeamBoardClient> logger,
    ICosmosDbService<Team> teamDbService,
    ICosmosDbService<TeamBoard> teamBoardDbService
) : ITeamBoardClient
{
    public async Task CreateTeamBoardAsync(CreateBoard boardData)
    {
        try
        {
            var teamBoard = new TeamBoard() { TeamBoardName = boardData.BoardName, };
            await teamBoardDbService.UpsertItemAsync(teamBoard);
            var query = $" SELECT * FROM c WHERE c.teamId = '{boardData.TeamId}' ";
            var team =
                (await teamDbService.GetItemsAsync(query)).FirstOrDefault()
                ?? throw new Exception("Invalid team id");
            team.TeamBoards.Add(teamBoard.TeamBoardId);
            await teamBoardDbService.UpsertItemAsync(teamBoard);
            await teamDbService.UpsertItemAsync(team);
        }
        catch (Exception ex)
        {
            logger.LogError("error in creating team board : {error}", ex.Message);
            throw new Exception("error in creating team board");
        }
    }

    public async Task<List<TeamBoard>> GetTeamBoardsAsync(List<string> boardIds)
    {
        try
        {
            var query =
                $" SELECT * FROM p WHERE p.teamBoardId IN ({String.Join(",", boardIds.Select(p => $" '{p}' "))}) ";
            var boards = await teamBoardDbService.GetItemsAsync(query);
            return boards.ToList();
        }
        catch (Exception ex)
        {
            logger.LogError("error in getting teamboards : {error}", ex.Message);
            throw new Exception("error getting teambaords");
        }
    }

    public async Task UpdateTeamBoardsAsync(TeamBoardUpdate teamBoardUpdate)
    {
        try { 
            var query = $" SELECT * FROM p WHERE p.teamBoardId = '{teamBoardUpdate.TeamBoardId}' ";
            var teamBoardResponse = (await teamBoardDbService.GetItemsAsync(query)).ToList();
            if(teamBoardResponse.IsNullOrEmpty()) throw new Exception("team boards not found");
            var teamboard = teamBoardResponse.First();
            teamboard.Data = teamBoardUpdate.Data;
            teamboard.TeamBoardName = teamBoardUpdate.TeamBoardName;
            teamboard.IsDeleted = teamBoardUpdate.IsDeleted;
            await teamBoardDbService.UpsertItemAsync(teamboard);
        }
        catch (Exception ex)
        {
            logger.LogError("error in updating team board : {error}", ex.Message);
            throw new Exception("error in updating team board");
        }
    }
}
