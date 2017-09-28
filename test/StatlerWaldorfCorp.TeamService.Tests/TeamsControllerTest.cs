using System;
using Xunit;
using StatlerWaldorfCorp.TeamService.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using StatlerWaldorfCorp.TeamService.Persisistence;

namespace StatlerWaldorfCorp.TeamService.Tests
{
    public class TeamControllerTest
    {
        
        
        [Fact]
        public void QueryTeamListREturnsCorrectTeams()
        {
            ITeamRepository repository = new  TestMemoryTeamRepository();
            TeamsController  controller = new TeamsController(repository);

            var rawTeams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> teams = new List<Team>(rawTeams);
            Assert.NotNull(teams); 
            Assert.Equal(teams.Count, 2);
            Assert.Equal(teams[0].Name, "one");
            Assert.Equal(teams[1].Name, "two");
        }

        [Fact]
        public  void CreateTeamAddsTeamToList()
        {
            ITeamRepository repository = new  TestMemoryTeamRepository();
            TeamsController controller = new TeamsController(repository);
            var teams = (IEnumerable<Team>)
             (controller.GetAllTeams() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Team t = new Team("sample");
            var result = controller.CreateTeam(t);
            Assert.Equal((result as ObjectResult).StatusCode, 201);

            var newTeamsRaw =
            (IEnumerable<Team>)
                (controller.GetAllTeams() as ObjectResult).Value;

            List<Team> newTeams = new List<Team>(newTeamsRaw);
            Assert.Equal(newTeams.Count, original.Count + 1);

            var sampleTeam = 
                newTeams.FirstOrDefault(
                    target => target.Name == "sample");


            Assert.NotNull(sampleTeam);
        }   

        [Fact]
        public void CreateMemberstoNonexisitantTeamReturnsNotFound()
        {
            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);

            Guid teamId = Guid.NewGuid();

            Guid newMemberId = Guid.NewGuid();
            Member newMember = new Member(newMemberId);
            var result = controller.CreateMember(newMember, teamId);

            Assert.True(result is NotFoundResult);
        }
    }
}
