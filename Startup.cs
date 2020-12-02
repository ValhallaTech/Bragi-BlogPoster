using BlogPosts.Data;
using BlogPosts.Models;
using BlogPosts.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogPosts
{
    public class Startup
    {
        public Startup( IConfiguration configuration ) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // . This method gets called by the runtime. Use this method to add services to the container.
        public void
            ConfigureServices(
                IServiceCollection services )
        {
            // Remember Dependencies Injection is set up here in the configureServices
            // 1. services are configured for using DbContext
            services.AddDbContext<ApplicationDbContext>(
                                                        options =>
                                                            options.UseNpgsql( // switched from defualt, to use NPGSql
                                                                              PostgreHelper.GetConnectionString(
                                                                               this.Configuration ) ) );

            // 2. using directive for injection using IdentityRole with BlogUser
            services.AddIdentity<BlogUser, IdentityRole>( options => options.SignIn.RequireConfirmedAccount = true )
                    .AddEntityFrameworkStores<ApplicationDbContext>( )
                    .AddDefaultUI( )
                    .AddDefaultTokenProviders( );

            services.AddControllersWithViews( );
            services.AddRazorPages( );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment( ) )
            {
                app.UseDeveloperExceptionPage( );
                app.UseDatabaseErrorPage( );
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts( );
            }

            app.UseHttpsRedirection( );
            app.UseStaticFiles( );

            app.UseRouting( );

            app.UseAuthentication( );
            app.UseAuthorization( );

            app.UseEndpoints(
                             endpoints =>
                             {
                                 endpoints.MapControllerRoute(
                                                              name: "default",
                                                              pattern: "{controller=Home}/{action=Index}/{id?}" );
                                 endpoints.MapRazorPages( );
                             } );
        }
    }
}
