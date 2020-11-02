using BlogPosts.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Linq;


namespace BlogPosts.Helpers
{

    public static class MigrateDb
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            try
            {
                using var scope = host.Services.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                if (pendingMigrations.Count > 0)
                {
                    var migrator = context.Database.GetService<IMigrator>();
                    foreach (var targetMigration in pendingMigrations)
                    {
                        migrator.Migrate(targetMigration);
                    }
                }
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex);
            }

            return host;
        }
    }
}
