using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Todo.WebApi.Data;

namespace Microsoft.Extensions.Hosting
{
    public static class TodoContextExtension
    {
        public static void MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var sp = scope.ServiceProvider;
            var logger = sp.GetRequiredService<ILoggerFactory>()
                           .CreateLogger<TodoContext>();
            try
            {
                logger.LogInformation("Migrate databases...");

                var todoContext = sp.GetRequiredService<TodoContext>();
                todoContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the DB.");
            }
        }
    }
}
