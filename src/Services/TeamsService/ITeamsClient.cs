using Codraw.Models.Team;
using Codraw.Models.TeamInteraction;

namespace Codraw.Services.TeamsService;

public interface ITeamsClient
{
    Task<List<Team>> GetTeamsByUserIdAsync(string userId);
    Task CreateTeamAsync(string teamName, string adminUserId);
    Task JoinTeamAsync(string teamId, string userId);
    Task RemoveTeamMemberAsync(RemoveTeamMember removeInfo);
}