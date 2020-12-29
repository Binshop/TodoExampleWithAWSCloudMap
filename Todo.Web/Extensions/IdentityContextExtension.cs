using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Todo.Web.Data;

namespace Microsoft.Extensions.Hosting
{
    public static class IdentityContextExtension
    {
        public static async Task SeedDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var sp = scope.ServiceProvider;
            var logger = sp.GetRequiredService<ILoggerFactory>()
                           .CreateLogger<IdentityContext>();
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
        }
    }
}
