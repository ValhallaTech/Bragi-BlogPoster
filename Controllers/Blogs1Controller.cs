using System.Linq;
using System.Threading.Tasks;
using BragiBlogPoster.Data;
using BragiBlogPoster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BragiBlogPoster.Controllers
{
    public class Blogs1Controller : Controller
    {
        private readonly ApplicationDbContext context;

        public Blogs1Controller(ApplicationDbContext context) => this.context = context;

        // GET: Blogs1
        public async Task<IActionResult> Index() => this.View(await this.context.Blog.ToListAsync().ConfigureAwait( false ) );

        // GET: Blogs1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Blog blog = await this.context.Blog
                                  .FirstOrDefaultAsync(m => m.Id == id)
                                  .ConfigureAwait( false );
            if (blog == null)
            {
                return this.NotFound();
            }

            return this.View(blog);
        }

        // GET: Blogs1/Create
        public IActionResult Create() => this.View();

        // POST: Blogs1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind( "Id,Name,Url")] Blog blog)
        {
            if ( this.ModelState.IsValid)
            {
                this.context.Add(blog);
                await this.context.SaveChangesAsync().ConfigureAwait( false );
                return this.RedirectToAction(nameof( this.Index));
            }

            return this.View(blog);
        }

        // GET: Blogs1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Blog blog = await this.context.Blog.FindAsync(id).ConfigureAwait( false );
            if (blog == null)
            {
                return this.NotFound();
            }

            return this.View(blog);
        }

        // POST: Blogs1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind( "Id,Name,Url")] Blog blog)
        {
            if (id != blog.Id)
            {
                return this.NotFound();
            }

            if ( this.ModelState.IsValid)
            {
                try
                {
                    this.context.Update(blog);
                    await this.context.SaveChangesAsync().ConfigureAwait( false );
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.BlogExists(blog.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof( this.Index));
            }

            return this.View(blog);
        }

        // GET: Blogs1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Blog blog = await this.context.Blog
                                  .FirstOrDefaultAsync(m => m.Id == id)
                                  .ConfigureAwait( false );
            if (blog == null)
            {
                return this.NotFound();
            }

            return this.View(blog);
        }

        // POST: Blogs1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Blog blog = await this.context.Blog.FindAsync(id).ConfigureAwait( false );
            this.context.Blog.Remove(blog);
            await this.context.SaveChangesAsync().ConfigureAwait( false );
            return this.RedirectToAction(nameof( this.Index));
        }

        private bool BlogExists(int id)
        {
            return this.context.Blog.Any(e => e.Id == id);
        }
    }
}
