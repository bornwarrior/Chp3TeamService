using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace StatlerWaldorfCorp.TeamService.Persisistence
{
    [Route("/teams/{teamId}/[controller]")]
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

        public Team Team(Team team)
        {
            teams.Add(team);
            return team;
        }

        public Team Get(Guid id)
        {
            return teams.FirstOrDefault( t => t.ID == id);
        } 

        public Team Add(Team team)
        {
            teams.Add(team);
            return team;
        }

        public Team UpdateTeam(Team t)
        {
            Team team = this.Delete(t.ID);

            if(team != null)
            {
                team = this.Add(t);
            }

            return team;
        }

        public Team Delete(Guid id)
        {
            var q = teams.Where(t => t.ID == id);
            Team team = null;

            if(q.Count() > 0)
            {
                team = q.First();
                teams.Remove(team);
            }

            return team;

        }
    }
}