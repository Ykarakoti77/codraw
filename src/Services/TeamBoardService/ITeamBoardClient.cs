using Codraw.Models.Team;
using Codraw.Models.TeamInteraction;

namespace Codraw.Services.TeamBoardService;

public interface ITeamBoardClient
{
    Task CreateTeamBoardAsync(CreateBoard boardData);
    Task<List<TeamBoard>> GetTeamBoardsAsync(List<string> boardIds);
    Task UpdateTeamBoardsAsync(TeamBoardUpdate teamBoard);
}
