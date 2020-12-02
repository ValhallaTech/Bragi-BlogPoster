using System.Threading.Tasks;
using BlogPosts.Enums;
using BlogPosts.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogPosts.Helpers
{
    public static class Seeder
    {
        public static async Task SeedDataAsync(
            UserManager<BlogUser>     userManager,
            RoleManager<IdentityRole> roleManager )
        {
            await SeedRolesAsync( roleManager ).ConfigureAwait( false );
            await SeedAdminAsync( userManager ).ConfigureAwait( false );
            await SeedModeratorAsync( userManager ).ConfigureAwait( false );
        }

        private static async Task SeedRolesAsync( RoleManager<IdentityRole> roleManager )
        {
            await roleManager.CreateAsync( new IdentityRole( nameof( Roles.Admin ) ) ).ConfigureAwait( false );
            await roleManager.CreateAsync( new IdentityRole( nameof( Roles.Moderator ) ) ).ConfigureAwait( false );
        }

        private static async Task SeedAdminAsync( UserManager<BlogUser> userManager )
        {
            if ( await userManager.FindByEmailAsync( "valhallatechnc@gmail.com" ).ConfigureAwait( false ) == null )
            {
                BlogUser admin = new BlogUser( )
                                 {
                                     Email          = "valhallatechtest+administrator@gmail.com",
                                     UserName       = "valhallatechtest+administrator@gmail.com",
                                     FirstName      = "Fred",
                                     LastName       = "Smith",
                                     EmailConfirmed = true
                                 };

                await userManager.CreateAsync( admin, "Abc&123!" ).ConfigureAwait( false );
                await userManager.AddToRoleAsync( admin, nameof( Roles.Admin ) ).ConfigureAwait( false );
            }
        }

        private static async Task SeedModeratorAsync( UserManager<BlogUser> userManager )
        {
            if ( await userManager.FindByEmailAsync( "smith.fred@yahoo.com" ).ConfigureAwait( false ) == null )
            {
                BlogUser moderator = new BlogUser( )
                                     {
                                         Email          = "valhallatechtest+projectmanager@gmail.com",
                                         UserName       = "valhallatechtest+projectmanager@gmail.com",
                                         FirstName      = "Fred",
                                         LastName       = "Smith",
                                         EmailConfirmed = true
                                     };
                await userManager.CreateAsync( moderator, "Abc&123!" ).ConfigureAwait( false );
                await userManager.AddToRoleAsync( moderator, nameof( Roles.Moderator ) ).ConfigureAwait( false );
            }
        }
    }
}
