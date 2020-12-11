using System.Linq;
using System.Threading.Tasks;
using BragiBlogPoster.Data;
using BragiBlogPoster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Z.EntityFramework.Plus;

namespace BragiBlogPoster.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext context;

        public CommentsController( ApplicationDbContext context ) => this.context = context;

        // GET: Comments
        [Authorize( Roles = "Administrator, Moderator" )]
        public async Task<IActionResult> Index( )
        {
            IIncludableQueryable<Comment, Post> applicationDbContext =
                this.context.Comment.IncludeOptimized( c => c.BlogUser ).IncludeOptimized( c => c.Post ) as IIncludableQueryable<Comment, Post>;

            return this.View( await applicationDbContext.ToListAsync( ).ConfigureAwait( false ) );
        }

        // GET: Comments/Details/5
        [Authorize( Roles = "Administrator, Moderator" )]
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Comment comment = await this.context.Comment
                                        .IncludeOptimized( c => c.BlogUser )
                                        .IncludeOptimized( c => c.Post )
                                        .FirstOrDefaultAsync( m => m.Id == id )
                                        .ConfigureAwait( false );

            if ( comment == null )
            {
                return this.NotFound( );
            }

            return this.View( comment );
        }

        // GET: Comments/Create
        [Authorize( Roles = "Administrator, Moderator" )]
        public IActionResult Create( )
        {
            this.ViewData["AuthorId"] = new SelectList( this.context.Set<BlogUser>( ), "Id", "Id" );
            this.ViewData["PostId"]   = new SelectList( this.context.Set<Post>( ),     "Id", "Id" );

            return this.View( );
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind( "Id,PostId,AuthorId,Content,Created,Updated" )]
            Comment comment )
        {
            if ( this.ModelState.IsValid )
            {
                await this.context.AddAsync( comment ).ConfigureAwait( false );
                await this.context.SaveChangesAsync( ).ConfigureAwait( false );

                return this.RedirectToAction( nameof( this.Index ) );
            }

            this.ViewData["AuthorId"] = new SelectList( this.context.Set<BlogUser>( ), "Id", "Id", comment.BlogUserId );
            this.ViewData["PostId"]   = new SelectList( this.context.Set<Post>( ),     "Id", "Id", comment.PostId );

            return this.View( comment );
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Comment comment = await this.context.Comment.FindAsync( id ).ConfigureAwait( false );

            if ( comment == null )
            {
                return this.NotFound( );
            }

            this.ViewData["AuthorId"] = new SelectList( this.context.Set<BlogUser>( ), "Id", "Id", comment.BlogUserId );
            this.ViewData["PostId"]   = new SelectList( this.context.Set<Post>( ),     "Id", "Id", comment.PostId );

            return this.View( comment );
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind( "Id,PostId,AuthorId,Content,Created,Updated" )]
            Comment comment )
        {
            if ( this.ModelState.IsValid )
            {
            }

            if ( id != comment.Id )
            {
                return this.NotFound( );
            }

            if ( this.ModelState.IsValid )
            {
                try
                {
                    this.context.Update( comment );
                    await this.context.SaveChangesAsync( ).ConfigureAwait( false );
                }
                catch ( DbUpdateConcurrencyException ) when ( !this.CommentExists( comment.Id ) )
                {
                    return this.NotFound( );
                }

                return this.RedirectToAction( nameof( this.Index ) );
            }

            this.ViewData["AuthorId"] = new SelectList( this.context.Set<BlogUser>( ), "Id", "Id", comment.BlogUserId );
            this.ViewData["PostId"]   = new SelectList( this.context.Set<Post>( ),     "Id", "Id", comment.PostId );

            return this.View( comment );
        }

        // GET: Comments/Delete/5
        [Authorize( Roles = "Administrator, Moderator" )]
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Comment comment = await this.context.Comment
                                        .IncludeOptimized( c => c.BlogUser )
                                        .IncludeOptimized( c => c.Post )
                                        .FirstOrDefaultAsync( m => m.Id == id )
                                        .ConfigureAwait( false );

            if ( comment == null )
            {
                return this.NotFound( );
            }

            return this.View( comment );
        }

        // POST: Comments/Delete/5
        [HttpPost]
        [ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        [Authorize( Roles = "Administrator, Moderator" )]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            Comment comment = await this.context.Comment.FindAsync( id ).ConfigureAwait( false );
            this.context.Comment.Remove( comment );
            await this.context.SaveChangesAsync( ).ConfigureAwait( false );

            return this.RedirectToAction( nameof( this.Index ) );
        }

        private bool CommentExists( int id )
        {
            return this.context.Comment.Any( e => e.Id == id );
        }
    }
}
