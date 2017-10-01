using Xunit;
using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using StatlerWaldorfCorp.TeamService.Persisistence;

[assembly:CollectionBehavior(MaxParallelThreads = 1)]

namespace StatlerWaldorfCorp.TeamService
{
    public class MembersControllerTest
    {
        [Fact]
        public void CreateMembersAddsTeamToList()
        {
            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);

            Guid teamId = Guid.NewGuid();
            Team team = new Team("TestController", teamId);
            repository.Add(team);

            Guid newMemberId = Guid.NewGuid();
            Member newMember = new Member(newMemberId);
            controller.CreateMember(newMember, teamId);

            team = repository.Get(teamId);
            Assert.True(team.Members.Contains(newMember));
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

        [Fact]
        public void GetExistingMemberReturnsMember()
        {
            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);

            Guid teamId = Guid.NewGuid();
            Team team = new Team("Test Team",teamId);
            var debugTeam = repository.Add(team);

            Guid memberId = Guid.NewGuid();
            Member newMember = new Member(memberId);
            newMember.FirstName = "Jim";
            newMember.LastName = "Smith";
            controller.CreateMember(newMember, teamId);

            Member member = (Member)(controller.GetMember(teamId,memberId) as ObjectResult).Value;
            Assert.Equal(newMember.ID, member.ID);
        }

        [Fact]
        public void GetMembersREturnsMembers()
        {
            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);

            Guid teamId = Guid.NewGuid();
            Team team = new Team("TestTeam", teamId);
            var debugTeam = repository.Add(team);

            Guid firstMemberId = Guid.NewGuid();
            Member newMember = new Member(firstMemberId);
            newMember.FirstName = "Jim";
            newMember.LastName = "Smith";
            controller.CreateMember(newMember, teamId);

            Guid secondMemeberId  = Guid.NewGuid();
            newMember = new Member(secondMemeberId);
            newMember.FirstName = "John";
            newMember.LastName = "Doe";
            controller.CreateMember(newMember,teamId);


            ICollection<Member> memebers = (ICollection<Member>)(controller.GetMember(teamId) as ObjectResult).Value;
            Assert.Equal(2, memebers.Count);
            Assert.NotNull(memebers.Where( m => m.ID == firstMemberId).First().ID);
            Assert.NotNull(memebers.Where( m => m.ID == secondMemeberId).First().ID);
        }

        [Fact]
        public void GetMembersForNewTeamIsEmpty()
        {

            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);

            Guid teamId = Guid.NewGuid();
            Team team = new Team("TestTeam", teamId);

            var debugTeam = repository.Add(team);

            ICollection<Member> members = (ICollection<Member>)(controller.GetMember(teamId) as ObjectResult).Value;
            Assert.Empty(members);
        }

        [Fact]
        public void GetMembersForNonExistantTEamREturnsNotFound()
        {
            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);

            var result = controller.GetMembers(Guid.NewGuid());
            Assert.True(result is NotFoundResult);


        }

        [Fact]
        public void GetNonExistantTeamREturnsNotFound()
        {

            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);

            var result = controller.GetMember(Guid.NewGuid(),Guid.NewGuid());
            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public void GetNonExistantMembersReturnsNotFound()
        {

            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);

            Guid teamId = Guid.NewGuid();
            Team team = new Team("TestTeam", teamId);
            var debugTeam = repository.Add(team);

            var result = controller.GetMember(teamId,Guid.NewGuid());
            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public void UpdateTeamberOverWrites()
        {

            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);


            Guid teamId = Guid.NewGuid();
            Team team = new Team("TestTeam", teamId);
            var debugTeam = repository.Add(team);


            Guid newMemberId = Guid.NewGuid();
            Member newMember = new Member(newMemberId);
            newMember.FirstName = "Jim";
            newMember.LastName = "Smith";
            controller.CreateMember(newMember, teamId);

            team = repository.Get(teamId);

            Member updateMember = new Member(newMemberId);
            updateMember.FirstName = "Bob";
            updateMember.LastName = "Jones";
            var result = controller.UpdateMember(updateMember, teamId, newMemberId);
            Assert.True( result is OkResult);

            team = repository.Get(teamId);
            Assert.Equal(1, team.Members.Count);
            Member testMember = team.Members.Where(m => m.ID == newMemberId).First();
            Assert.NotNull(testMember);
            Assert.Equal("Bob",  testMember.FirstName);
            Assert.Equal("Jones", testMember.LastName);

        }
        [Fact]
        public void UpdateMemeberToNonExistantMembersReturnsNoMatch()
        {

            ITeamRepository repository = new TestMemoryTeamRepository();
            MembersController controller = new MembersController(repository);


            Guid teamId = Guid.NewGuid();
            Team team = new Team("TestController", teamId);
            repository.Add(team);

            Guid memberId = Guid.NewGuid();

            Member newMember = new Member(memberId);

            newMember.FirstName = "Jim";
            newMember.LastName = "Smith";
            controller.CreateMember(newMember, teamId);

            Guid nonMatchdGuid = Guid.NewGuid();
            Member updateMember = new Member(nonMatchdGuid);
            updateMember.FirstName = "Bob";
            var result = controller.UpdateMember(updateMember,teamId, nonMatchdGuid);

            Assert.True(result is NotFoundResult);


        }
    }
}