using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;
using System;
using System.Linq;

namespace StatlerWaldorfCorp.TeamService.Persisistence
{
    public class MemoryTeamRepository : ITeamRepository
    {
        protected static ICollection<Team> teams;

        public MemoryTeamRepository()
        {
            if(teams == null) {
                teams = new List<Team>();
            }
        }

        public MemoryTeamRepository(ICollection<Team> teams)
        {
            MemoryTeamRepository.teams = teams;
        }

        public IEnumerable<Team> GetTeams() {
            return teams;
        }

        public Team AddTeam(Team team)
        {
            teams.Add(team);
            return team;
        }

        public Team Get(Guid id)
        {
            return teams.FirstOrDefault( t => t.ID == id);
        } 
    }
}