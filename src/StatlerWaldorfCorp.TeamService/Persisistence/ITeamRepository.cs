using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;
using System;

namespace StatlerWaldorfCorp.TeamService.Persisistence
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams();

        Team AddTeam(Team team);
        Team Get(Guid id);

        
    }
}