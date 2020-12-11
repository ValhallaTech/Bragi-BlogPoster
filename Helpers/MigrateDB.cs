// Deprecated

using System;
using System.Collections.Generic;
using System.Linq;
using BragiBlogPoster.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace BragiBlogPoster.Helpers
{
    public static class MigrateDb
    {
        public static IHost MigrateDatabase( this IHost host )
        {
            try
            {
                using IServiceScope        scope   = host.Services.CreateScope( );
                using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>( );

                List<string> pendingMigrations = context.Database.GetPendingMigrations( ).ToList( );

                if ( pendingMigrations.Count > 0 )
                {
                    IMigrator migrator = context.Database.GetService<IMigrator>( );
                    foreach ( string targetMigration in pendingMigrations )
                    {
                        migrator.Migrate( targetMigration );
                    }
                }
            }
            catch ( PostgresException ex )
            {
                Console.WriteLine( ex );
            }

            return host;
        }
    }
}
