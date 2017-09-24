using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using StatlerWaldorfCorp.TeamService.Models;

namespace StatlerWaldorfCorp.TeamService
{
    public class  TeamController
    {
        public TeamController()
        {
            
        }

        [HttpGet]
        public IEnumerable<Team> GetAllTeams()
        {
            return new Team[] {new Team("one"), new Team("two")};
        }
    }
}