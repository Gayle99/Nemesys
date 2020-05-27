using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nemesys.Data;
using Nemesys.Models;
using NLog.Web;

namespace Nemesys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                var services = scope.ServiceProvider;
                //try
                //{
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                    DbInitalizer.SeedRoles(roleManager);
                    DbInitalizer.SeedUsers(userManager);
                    DbInitalizer.SeedData(userManager, context);
                    logger.Debug("init main");
                    CreateHostBuilder(args).Build().Run();
                //}
                //catch(Exception e)
                //{
                //    logger.Error(e, "An unexpected error occured during the seeding stage!");
                //}
                //}
                //catch(Exception e)
                //{
                    //logging
                //}
                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            }).UseNLog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
