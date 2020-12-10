using System.Linq;
using System.Threading.Tasks;
using BragirBlogPoster.Data;
using BragirBlogPoster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BragirBlogPoster.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext context;

        public TagsController( ApplicationDbContext context ) => this.context = context;

        // GET: Tags
        public async Task<IActionResult> Index( ) =>
            this.View( await this.context.Tags.ToListAsync( ).ConfigureAwait( false ) );

        // GET: Tags/Details/5
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Tag tag = await this.context.Tags.FirstOrDefaultAsync( m => m.Id == id ).ConfigureAwait( false );

            if ( tag == null )
            {
                return this.NotFound( );
            }

            return this.View( tag );
        }

        // GET: Tags/Create
        public IActionResult Create( ) => this.View( );

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind( "Id,Name" )] Tag tag )
        {
            if ( this.ModelState.IsValid )
            {
                await this.context.AddAsync( tag ).ConfigureAwait( false );
                await this.context.SaveChangesAsync( ).ConfigureAwait( false );

                return this.RedirectToAction( nameof( this.Index ) );
            }

            return this.View( tag );
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Tag tag = await this.context.Tags.FindAsync( id ).ConfigureAwait( false );

            if ( tag == null )
            {
                return this.NotFound( );
            }

            return this.View( tag );
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int id, [Bind( "Id,Name" )] Tag tag )
        {
            if ( id != tag.Id )
            {
                return this.NotFound( );
            }

            if ( this.ModelState.IsValid )
            {
                try
                {
                    this.context.Update( tag );
                    await this.context.SaveChangesAsync( ).ConfigureAwait( false );
                }
                catch ( DbUpdateConcurrencyException ) when ( !this.TagExists( tag.Id ) )
                {
                    return this.NotFound( );
                }

                return this.RedirectToAction( nameof( this.Index ) );
            }

            return this.View( tag );
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null )
            {
                return this.NotFound( );
            }

            Tag tag = await this.context.Tags.FirstOrDefaultAsync( m => m.Id == id ).ConfigureAwait( false );

            if ( tag == null )
            {
                return this.NotFound( );
            }

            return this.View( tag );
        }

        // POST: Tags/Delete/5
        [HttpPost]
        [ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            Tag tag = await this.context.Tags.FindAsync( id ).ConfigureAwait( false );
            this.context.Tags.Remove( tag );
            await this.context.SaveChangesAsync( ).ConfigureAwait( false );

            return this.RedirectToAction( nameof( this.Index ) );
        }

        private bool TagExists( int id )
        {
            return this.context.Tags.Any( e => e.Id == id );
        }
    }
}
