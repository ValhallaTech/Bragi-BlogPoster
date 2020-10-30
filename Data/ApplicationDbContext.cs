﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogPosts.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BlogPosts.Models.Blog> Blog { get; set; }
        public DbSet<BlogPosts.Models.Comment> Comment { get; set; }
        public DbSet<BlogPosts.Models.Post> Post { get; set; }
        public DbSet<BlogPosts.Models.Tag> Tag { get; set; }
    }
}