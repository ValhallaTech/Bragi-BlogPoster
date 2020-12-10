using System;
using System.Threading.Tasks;
using BragirBlogPoster.Data;
using BragirBlogPoster.Helpers;
using BragirBlogPoster.Models;
using BragirBlogPoster.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BragirBlogPoster
{
    public static class Program
    {
        public static async Task Main( string[] args )
        {
            IHost host = CreateHostBuilder( args ).Build( );
            await PostgreHelper.ManageDataAsync( host ).ConfigureAwait( false );
            await SeedDataAsync( host ).ConfigureAwait( false );
            await host.RunAsync( ).ConfigureAwait( false );
        }

        private static IHostBuilder CreateHostBuilder( string[] args ) =>
            Host.CreateDefaultBuilder( args )
                .ConfigureWebHostDefaults(
                                          webBuilder =>
                                          {
                                              webBuilder.CaptureStartupErrors( true );
                                              webBuilder.UseSetting( WebHostDefaults.DetailedErrorsKey, "true" );
                                              webBuilder.UseStartup<Startup>( );
                                          } );

        private static async Task SeedDataAsync( IHost host )
        {
            using IServiceScope scope    = host.Services.CreateScope( );
            IServiceProvider    services = scope.ServiceProvider;

            try
            {
                UserManager<BlogUser>     userManager = services.GetRequiredService<UserManager<BlogUser>>( );
                RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>( );
                await DatabaseSeeder.SeedDataAsync( userManager, roleManager ).ConfigureAwait( false );
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex );
            }
        }
    }
}
