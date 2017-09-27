using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;

namespace StatlerWaldorfCorp.TeamService.Persisistence
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams();
        void AddTeam(Team team);
        
    }
}