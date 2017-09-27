using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StatlerWaldorfCorp.TeamService.Persisistence;

namespace  StatlerWaldorfCorp.TeamService
{
  public class Startup
  {
      public Startup(IHostingEnvironment env)
      {
      }        
       
      public void Configure(IApplicationBuilder app, 
        IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
        app.Run(async (context) =>
        {
          await context.Response.WriteAsync("Hello, world!");
        });
      }

      public void ConfigureService(IServiceCollection service)
      {
          service.AddMvc();
          service.AddScoped<ITeamRepository, MemoryTeamRepository>();
      }
  }   
}
