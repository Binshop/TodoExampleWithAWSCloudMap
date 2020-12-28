using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Todo.Web.Data;

namespace Todo.Web
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var sp = scope.ServiceProvider;
            var logger = sp.GetRequiredService<ILoggerFactory>()
                           .CreateLogger<LambdaEntryPoint>();
            try
            {
                logger.LogInformation("Migrate databases...");

                var identityContext = sp.GetRequiredService<IdentityContext>();
                identityContext.Database.Migrate();

                logger.LogInformation("Seeding data into databases...");
                await IdentityContextSeed.SeedAsync(sp);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the DB.");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
