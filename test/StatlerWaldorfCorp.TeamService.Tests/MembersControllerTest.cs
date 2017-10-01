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

    }
}