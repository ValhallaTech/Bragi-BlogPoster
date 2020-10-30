using BlogPosts.Enums;
using BlogPosts.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlogPosts.Helpers
{
    public static class Seeder
    {
        public static async Task SeedDataAsync(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            await SeedModerator(userManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        }

        private static async Task SeedAdmin(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("valhallatechnc@gmail.com") == null)

            {
                var admin = new BlogUser()
                                {
                                    Email          = "valhallatechnc@gmail.com",
                                    UserName       = "valhallatechnc@gmail.com",
                                    FirstName      = "Fred",
                                    LastName       = "Smith",
                                    EmailConfirmed = true
                                };

                await userManager.CreateAsync(admin, "Abc&123!");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }

        private static async Task SeedModerator(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("smith.fred@yahoo.com") == null)
            {
                var moderator = new BlogUser()
                                    {
                                        Email          = "smith.fred@yahoo.com",
                                        UserName       = "smith.fred@yahoo.com",
                                        FirstName      = "Fred",
                                        LastName       = "Smith",
                                        EmailConfirmed = true
                                    };
                await userManager.CreateAsync(moderator, "Abc&123!");
                await userManager.AddToRoleAsync(moderator, Roles.Moderator.ToString());
            }
        }
    }
}


