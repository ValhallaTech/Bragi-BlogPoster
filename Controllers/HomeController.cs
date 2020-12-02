using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogPosts.Data;
using BlogPosts.Models;
using BlogPosts.Models.ViewModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace MVCBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        private readonly ILogger<HomeController> logger;

        public HomeController( ILogger<HomeController> logger, ApplicationDbContext context )
        {
            this.logger  = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index( int page )
        {
            int posted = this.context.Post.Where( p => p.IsPublished ).ToList( ).Count;

            if ( page < 0 )
            {
                page = 0;
            }

            if ( page * 5 > this.context.Post.ToList( ).Count )
            {
                page = this.context.Post.ToList( ).Count / 5;
            }

            IQueryable<Post> posts = this.context.Post.Where( p => p.IsPublished )
                                         .OrderByDescending( p => p.CreatedDateTime )
                                         .Include( p => p.Blog )
                                         .Skip( page * 5 )
                                         .Take( 5 );
            DbSet<Blog> blogs = this.context.Blog;
            var     tags  = this.context.Tag;
            CategoryViewModel categories = new CategoryViewModel( )
                                           {
                                               Blogs      = await blogs.ToListAsync( ).ConfigureAwait( false ),
                                               Posts      = await posts.ToListAsync( ).ConfigureAwait( false ),
                                               Tags       = await tags.ToListAsync( ).ConfigureAwait( false ),
                                               PageNum    = page,
                                               TotalPosts = this.context.Post.ToList( ).Count
                                           };

            return View( categories );
        }

        public async Task<IActionResult> Results( string searchString )
        {
            IQueryable<Post> posts = from p in this.context.Post select p;
            DbSet<Blog>      blogs = this.context.Blog;
            var              tags  = this.context.Tag;

            if ( !string.IsNullOrEmpty( searchString ) )
            {
                posts = posts.Where(
                                    p => p.Title.Contains( searchString )
                                      || p.Abstract.Contains( searchString )
                                      || p.Content.Contains( searchString ) );

                // return View("Index", await posts.Include(p => p.Blog).ToListAsync());
            }

            // return View("Index", await posts.Include(p => p.Blog).ToListAsync());
            CategoryViewModel categories = new CategoryViewModel( )
                                           {
                                               Blogs = await blogs.ToListAsync( ).ConfigureAwait( false ),
                                               Posts = await posts.ToListAsync( ).ConfigureAwait( false ),
                                               Tags  = await tags.ToListAsync( ).ConfigureAwait( false )
                                           };

            return this.View( "Index", categories );
        }

        public async Task<IActionResult> Categories( )
        {
            string? id = RouteData.Values["id"].ToString( );
            IIncludableQueryable<Post, Blog> posts = this.context.Post.Where( p => p.BlogId == int.Parse( id ) && p.IsPublished == true )
                                                         .Include( p => p.Blog );
            DbSet<Blog> blogs = this.context.Blog;
            DbSet<Tag>  tags  = this.context.Tags;
            CategoryViewModel categories = new CategoryViewModel( )
                                           {
                                               Blogs = await blogs.ToListAsync( ).ConfigureAwait( false ),
                                               Posts = await posts.ToListAsync( ).ConfigureAwait( false ),
                                               Tags  = await tags.ToListAsync( ).ConfigureAwait( false )
                                           };

            return this.View( "Index", categories );
        }

        public async Task<IActionResult> Tag( )
        {
            string?            name  = RouteData.Values["id"].ToString( );
            IQueryable<string> tags  = this.context.Tags.Where( t => t.Name == name ).Select( t => t.Post );
            DbSet<Post>        posts = this.context.Post;
            DbSet<Blog>        blogs = this.context.Blog;
            CategoryViewModel categories = new CategoryViewModel( )
                                           {
                                               Blogs = await blogs.ToListAsync( ).ConfigureAwait( false ),
                                               Posts = await posts.ToListAsync( ).ConfigureAwait( false ),
                                               Tags  = await tags.ToListAsync( ).ConfigureAwait( false )
                                           };

            return this.View( "Index", categories );
        }

        public IActionResult Privacy( )
        {
            return this.View( );
        }

        [ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
        public IActionResult Error( )
        {
            return this.View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }
    }
}
