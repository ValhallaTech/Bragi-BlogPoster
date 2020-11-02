using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogPosts.Data;
using BlogPosts.Models;
using System.Security.Claims;

namespace BlogPosts.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext context;

        public CommentsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this.context.Comment.Include(c => c.Author).Include(c => c.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await this.context.Comment
                                    .Include(c => c.Author)
                                    .Include(c => c.Post)
                                    .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(this.context.Set<BlogUser>(), "Id", "Id");
            ViewData["PostId"]   = new SelectList(this.context.Set<Post>(),     "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] [ValidateAntiForgeryToken] public async Task<IActionResult> Create(
            [Bind("Id,PostId,AuthorId,Content,Created,Updated")]
            Comment comment)
        {
            if (ModelState.IsValid)
            {
                this.context.Add(comment);
                await this.context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(this.context.Set<BlogUser>(), "Id", "Id", comment.AuthorId);
            ViewData["PostId"]   = new SelectList(this.context.Set<Post>(),     "Id", "Id", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await this.context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            ViewData["AuthorId"] = new SelectList(this.context.Set<BlogUser>(), "Id", "Id", comment.AuthorId);
            ViewData["PostId"]   = new SelectList(this.context.Set<Post>(),     "Id", "Id", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] [ValidateAntiForgeryToken] public async Task<IActionResult> Edit(
            int id, [Bind("Id,PostId,AuthorId,Content,Created,Updated")]
            Comment comment)
        {
            if (ModelState.IsValid)
            {
            }

            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.context.Update(comment);
                    await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(this.context.Set<BlogUser>(), "Id", "Id", comment.AuthorId);
            ViewData["PostId"]   = new SelectList(this.context.Set<Post>(),     "Id", "Id", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await this.context.Comment
                                    .Include(c => c.Author)
                                    .Include(c => c.Post)
                                    .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")] [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await this.context.Comment.FindAsync(id);
            this.context.Comment.Remove(comment);
            await this.context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return this.context.Comment.Any(e => e.Id == id);
        }
    }
}