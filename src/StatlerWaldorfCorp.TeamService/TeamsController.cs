using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using StatlerWaldorfCorp.TeamService.Models;
using StatlerWaldorfCorp.TeamService.Persisistence;
 

namespace StatlerWaldorfCorp.TeamService
{
    public class  TeamsController : Controller
    {
    
        ITeamRepository repository;

        public TeamsController(ITeamRepository repo)
        {
            repository = repo;
        }
 
        [HttpGet]
        //public async virtual Task<IActionResult> GetAllTeams()
        public virtual IActionResult GetAllTeams()
        {
            return this.Ok(repository.GetTeams());

        }

        [HttpPost]
        public virtual IActionResult CreateTeam([FromBody]Team newTeam)
        {
            repository.AddTeam(newTeam);
            
            return this.Created($"/teams/{newTeam.ID}",newTeam);
        }
    }
}