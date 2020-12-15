using System.Linq;
using System.Threading.Tasks;
using BragiBlogPoster.Data;
using BragiBlogPoster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BragiBlogPoster.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext context;

        public BlogsController( ApplicationDbContext context ) => this.context = context;

        // GET: Blogs
        public async Task<IActionResult> Index( ) => this.View( await this.context.Blog.ToListAsync( ).ConfigureAwait( false ) );

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Blog blog = await this.context.Blog.FirstOrDefaultAsync( m => m.Id == id ).ConfigureAwait( false );

            if ( blog == null )
            {
                return this.NotFound( );
            }

            return this.View( blog );
        }

        [Authorize]
        // GET: Blogs/Create
        public IActionResult Create( ) => this.View( );

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind( "Id,Name,Url" )] Blog blog )
        {
            if ( this.ModelState.IsValid )
            {
                await this.context.AddAsync( blog ).ConfigureAwait( false );
                await this.context.SaveChangesAsync( ).ConfigureAwait( false );

                return this.RedirectToAction( nameof( this.Index ) );
            }

            return this.View( blog );
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Blog blog = await this.context.Blog.FindAsync( id ).ConfigureAwait( false );

            if ( blog == null )
            {
                return this.NotFound( );
            }

            return this.View( blog );
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int id, [Bind( "Id,Name,Url" )] Blog blog )
        {
            if ( id != blog.Id )
            {
                return this.NotFound( );
            }

            if ( this.ModelState.IsValid )
            {
                try
                {
                    this.context.Update( blog );
                    await this.context.SaveChangesAsync( ).ConfigureAwait( false );
                }
                catch ( DbUpdateConcurrencyException ) when ( !this.BlogExists( blog.Id ) )
                {
                    return this.NotFound( );
                }

                return this.RedirectToAction( nameof( this.Index ) );
            }

            return this.View( blog );
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Blog blog = await this.context.Blog.FirstOrDefaultAsync( m => m.Id == id ).ConfigureAwait( false );

            if ( blog == null )
            {
                return this.NotFound( );
            }

            return this.View( blog );
        }

        // POST: Blogs/Delete/5
        [HttpPost]
        [ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            Blog blog = await this.context.Blog.FindAsync( id ).ConfigureAwait( false );
            this.context.Blog.Remove( blog );
            await this.context.SaveChangesAsync( ).ConfigureAwait( false );

            return this.RedirectToAction( nameof( this.Index ) );
        }

        private bool BlogExists( int id )
        {
            return this.context.Blog.Any( e => e.Id == id );
        }
    }
}
