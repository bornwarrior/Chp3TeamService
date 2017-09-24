using System;
using Xunit;
using StatlerWaldorfCorp.TeamService.Models;
using System.Collections.Generic;

namespace StatlerWaldorfCorp.TeamService.Tests
{
    public class TeamControllerTest
    {
        TeamController  controller = new TeamController();
        [Fact]
        public void QueryTeamListREturnsCorrectTeams()
        {
            List<Team> teams = new List<Team>(
                controller.GetAllTeams());
            Assert.Equal(teams.Count, 2);
        }
    }
}
