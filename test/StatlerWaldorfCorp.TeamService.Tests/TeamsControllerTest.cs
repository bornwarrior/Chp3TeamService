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
        public void GetTeamRetrievesTeam()
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());

            string sampleName = "sample";
            Guid id = Guid.NewGuid();
            Team sampleTeam = new Team(sampleName,id);
            controller.CreateTeam(sampleTeam);

            Team retrievedTeam = (Team)(controller.GetTeam(id) as ObjectResult).Value;
            Assert.Equal(sampleName, retrievedTeam.Name);
            Assert.Equal(retrievedTeam.ID, id);
        }
        
        [Fact]
        public void GetNonExistentTeamReturnsNotFound()
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());

            Guid id = Guid.NewGuid();
            var result = controller.GetTeam(id);
            Assert.True(result is NotFoundResult);
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
        public void UpdateTeamModifiesTeamToList()
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Guid id = Guid.NewGuid();
            Team t = new Team("sample",id);
            var result = controller.CreateTeam(t);

            Team newTeam = new Team("sample2", id);
            controller.UpdateTeam(newTeam, id);

            var newTeamsRaw = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> newTeams = new List<Team>(newTeamsRaw);
            var sampleTeam = newTeams.FirstOrDefault( target => target.Name == "sample");
            Assert.Null(sampleTeam); // sample is updated so should be null

            Team retrievedTeam = (Team)(controller.GetTeam(id) as ObjectResult).Value;
            Assert.Equal("sample2", retrievedTeam.Name);

        }

        [Fact]
        public void UpdateNonExistentTeamReturnsNotFound()
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var teams =(IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Team someteam = new Team("Some Team", Guid.NewGuid());
            controller.CreateTeam(someteam);

            Guid newTeamId = Guid.NewGuid();
            Team newTeam = new Team("new team", newTeamId);

            var result = controller.UpdateTeam(newTeam, newTeamId);

            Assert.True(result is NotFoundResult);



        }   

        [Fact]
        public void DeleteTeamRemovesFromList()
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            int ct = teams.Count();

            string sampleName = "sample";
            Guid id = Guid.NewGuid();
            Team sampleTeam = new Team(sampleName, id);
            controller.CreateTeam(sampleTeam);

            
            teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            sampleTeam = teams.FirstOrDefault(target => target.Name == sampleName);
            Assert.NotNull(sampleTeam);

            controller.DeleteTeam(id);
            teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            sampleTeam = teams.FirstOrDefault(target => target.Name == sampleName);
            Assert.Null(sampleTeam);

        }

        [Fact]
        public void DeleteNonExistentTEamREturnsNotFound()
        {
            
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            Guid id = Guid.NewGuid();

            var result = controller.DeleteTeam(id);
            Assert.True(result is NotFoundResult);

        }
        // [Fact]
        // public void CreateMemberstoNonexisitantTeamReturnsNotFound()
        // {
        //     ITeamRepository repository = new TestMemoryTeamRepository();
        //     MembersController controller = new MembersController(repository);

        //     Guid teamId = Guid.NewGuid();

        //     Guid newMemberId = Guid.NewGuid();
        //     Member newMember = new Member(newMemberId);
        //     var result = controller.CreateMember(newMember, teamId);

        //     Assert.True(result is NotFoundResult);
        // }
        


        // [Fact]
        // public void GetExistingMemberReturnsMember()
        // {
        //     ITeamRepository repository = new TestMemoryTeamRepository();
        //     MembersController controller = new MembersController(repository);

        //     Guid teamId = Guid.NewGuid();
        //     Team team = new Team("Test Team",teamId);
        //     var debugTeam = repository.Add(team);

        //     Guid memberId = Guid.NewGuid();
        //     Member newMember = new Member(memberId);
        //     newMember.FirstName = "Jim";
        //     newMember.LastName = "Smith";
        //     controller.CreateMember(newMember, teamId);

        //     Member member = (Member)(controller.GetMember(teamId,memberId) as ObjectResult).Value;
        //     Assert.Equal(newMember.ID, member.ID);
        // }
    }
}
