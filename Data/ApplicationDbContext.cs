using BragirBlogPoster.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BragirBlogPoster.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
            : base( options )
        {
        }

        public DbSet<Blog> Blog { get; set; }

        public DbSet<BlogUser> BlogUser { get; set; }

        public DbSet<Post> Post { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}
