using Codraw.Framework.DbCore;
using Codraw.Models.Team;
using Codraw.Models.TeamInteraction;
using Microsoft.IdentityModel.Tokens;

namespace Codraw.Services.TeamsService;

public class TeamsClient(ILogger<TeamsClient> logger, ICosmosDbService<Team> teamDbService)
    : ITeamsClient
{
    public async Task CreateTeamAsync(string teamName, string adminUserId)
    {
        try
        {
            var team = new Team()
            {
                TeamName = teamName,
                TeamMembers = [new() { UserId = adminUserId, Role = Role.ADMIN }],
            };

            await teamDbService.UpsertItemAsync(team);
        }
        catch (Exception ex)
        {
            logger.LogError("error in creating team : {error}", ex.Message);
            throw new Exception("error in creating team");
        }
    }

    public async Task<List<Team>> GetTeamsByUserIdAsync(string userId)
    {
        try{
            string query = $" SELECT * FROM c WHERE ARRAY_CONTAINS(c.teamMembers, {{ 'userId': '{userId}' }}, true) ";
            var teams = await teamDbService.GetItemsAsync(query);
            return teams.ToList();
        }
        catch(Exception ex)
        {
            logger.LogError("error in creating team : {error}", ex.Message);
            throw new Exception("error in gettings teams");
        }
    }

    public async Task JoinTeamAsync(string teamInviteId, string userId)
    {
        try
        {
            var query = $" SELECT * FROM c WHERE c.teamInviteId = '{teamInviteId}' ";
            var teams = await teamDbService.GetItemsAsync(query);
            if (teams.IsNullOrEmpty())
                throw new Exception("Team not found");
            var team = teams.First();
            bool userExists = team.TeamMembers.Any(member => member.UserId == userId);
            if (!userExists)
            {
                team.TeamMembers.Add(new() { UserId = userId, Role = Role.MEMBER });
                await teamDbService.UpsertItemAsync(team);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("error in joining team : {error}", ex.Message);
            throw new Exception("error in joining team");
        }
    }

    public async Task RemoveTeamMemberAsync(RemoveTeamMember removeInfo)
    {
        try
        {
            var query = $"SELECT * FROM c WHERE c.teamId = '{removeInfo.TeamId}'";
            var teams = await teamDbService.GetItemsAsync(query);
            var team = teams.FirstOrDefault() ?? throw new Exception("Team not found");

            var user = team.TeamMembers.FirstOrDefault(member => member.UserId == removeInfo.UserId);
            var remover = team.TeamMembers.FirstOrDefault(member => member.UserId == removeInfo.RemoverId);

            if (user == null || remover == null)
            {
                throw new Exception("User doesn't exist");
            }

            if (remover.Role == Role.ADMIN || removeInfo.UserId == removeInfo.RemoverId)
            {
                team.TeamMembers.Remove(user);
                await teamDbService.UpsertItemAsync(team);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("error in removing team member : {error}", ex.Message);
            throw new Exception("error in removing team member");
        }
    }
}
