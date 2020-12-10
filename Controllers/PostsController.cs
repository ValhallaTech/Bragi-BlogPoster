#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BragirBlogPoster.Data;
using BragirBlogPoster.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Z.EntityFramework.Plus;

namespace BragirBlogPoster.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext context;

        public PostsController( ApplicationDbContext context ) => this.context = context;

        // Shows individual posts
        public async Task<IActionResult> BlogPosts( int? id )
        {
            // I know that I will have to push an instance of Post into the View
            if ( id == null )
            {
                return this.NotFound( );
            }

            Blog blog = await this.context.Blog.FindAsync( id ).ConfigureAwait( false );

            if ( blog == null )
            {
                return this.NotFound( );
            }

            this.ViewData["BlogName"] = blog.Name;
            this.ViewData["BlogId"]   = blog.Id;

            IQueryable<Post> posts = this.context.Post.Where( p => p.BlogId == id );

            // Linking my posts to topic
            return this.View( await posts.ToListAsync( ).ConfigureAwait( false ) );
        }

        // GET: Posts
        public async Task<IActionResult> Index( )
        {
            IIncludableQueryable<Post, Blog> applicationDbContext = this.context.Post.Include( p => p.Blog );

            return this.View( await applicationDbContext.ToListAsync( ).ConfigureAwait( false ) );
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Post post = await this.context.Post.IncludeOptimized( p => p.Blog )
                                  .FirstOrDefaultAsync( m => m.Id == id )
                                  .ConfigureAwait( false );

            if ( post == null )
            {
                return this.NotFound( );
            }

            string  binary    = Convert.ToBase64String( post.Image );
            string? ext       = Path.GetExtension( post.FileName );
            string  imageData = $"data:image/{ext};base64,[binary]";

            this.ViewData["Image"] = imageData;

            return this.View( post );
        }

        // GET: Posts/Create
        public IActionResult Create( int? id )
        {
            Post post = null;

            if ( id != null )
            {
                Blog blog = this.context.Blog.Find( id );

                if ( blog == null )
                {
                    return this.NotFound( );
                }

                post                      = new Post { BlogId = ( int )id };
                this.ViewData["BlogName"] = blog.Name;
            }
            else
            {
                this.ViewData["BlogId"] = new SelectList( this.context.Blog, "Id", "Name" );
            }

            return this.View( post );
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind( "Id,BlogId,Title,Abstract,Content,Created,Updated" )]
            Post post,
            IFormFile image )
        {
            if ( this.ModelState.IsValid )
            {
                post.CreatedDateTime = DateTime.UtcNow;

                if ( true )
                {
                    post.FileName = image.FileName;
                    MemoryStream ms = new MemoryStream( );
                    await image.CopyToAsync( ms ).ConfigureAwait( false );
                    post.Image = ms.ToArray( );
                    ms.Close( );
                    await ms.DisposeAsync( ).ConfigureAwait( false );
                }
                else
                {
                    await this.context.AddAsync( post ).ConfigureAwait( false );
                    await this.context.SaveChangesAsync( ).ConfigureAwait( false );

                    return this.RedirectToAction( nameof( this.Index ) );
                }

                this.ViewData["BlogId"] = new SelectList( this.context.Blog, "Id", "Id", post.BlogId );
            }

            return this.View( post );
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Post post = await this.context.Post.FindAsync( id ).ConfigureAwait( false );

            if ( post == null )
            {
                return this.NotFound( );
            }

            this.ViewData["BlogId"] = new SelectList( this.context.Blog, "Id", "Id", post.BlogId );

            return this.View( post );
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind( "Id,BlogId,Title,Abstract,Content,Image,Created,Updated" )]
            Post post )
        {
            if ( id != post.Id )
            {
                return this.NotFound( );
            }

            if ( this.ModelState.IsValid )
            {
                try
                {
                    this.context.Update( post );
                    await this.context.SaveChangesAsync( ).ConfigureAwait( false );
                }
                catch ( DbUpdateConcurrencyException ) when ( !this.PostExists( post.Id ) )
                {
                    return this.NotFound( );
                }

                return this.RedirectToAction( nameof( this.Index ) );
            }

            this.ViewData["BlogId"] = new SelectList( this.context.Blog, "Id", "Id", post.BlogId );

            return this.View( post );
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Post post = await this.context.Post.IncludeOptimized( p => p.Blog )
                                  .FirstOrDefaultAsync( m => m.Id == id )
                                  .ConfigureAwait( false );

            if ( post == null )
            {
                return this.NotFound( );
            }

            return this.View( post );
        }

        // POST: Posts/Delete/5
        [HttpPost]
        [ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            Post post = await this.context.Post.FindAsync( id ).ConfigureAwait( false );
            this.context.Post.Remove( post );
            await this.context.SaveChangesAsync( ).ConfigureAwait( false );

            return this.RedirectToAction( nameof( this.Index ) );
        }

        private bool PostExists( int id )
        {
            return this.context.Post.Any( e => e.Id == id );
        }
    }
}
