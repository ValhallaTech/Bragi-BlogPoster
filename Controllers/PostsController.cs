using BlogPosts.Data;
using BlogPosts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPosts.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext context;

        public PostsController(ApplicationDbContext context)
        {
            this.context = context;
        }


        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this.context.Post.Include(p => p.Blog);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await this.context.Post
                                 .Include(p => p.Blog)
                                 .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var    binary    = Convert.ToBase64String(post.Image);
            var    ext       = Path.GetExtension(post.FileName);
            string imageData = $"data:image/{ext};base64,[binary]";

            ViewData["Image"] = imageData;
            return View(post);
        }


        // GET: Posts/Create
        public IActionResult Create(int? id)
        {
            Post post = null;
            if (id != null)
            {
                var blog = this.context.Blog.Find(id);
                if (blog == null)
                {
                    return NotFound();
                }

                post                 = new Post() {BlogId = (int) id};
                ViewData["BlogName"] = blog.Name;
            }
            else
            {
                ViewData["BlogId"] = new SelectList(this.context.Blog, "Id", "Name");
            }

            return View(post);
        }


        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost] [ValidateAntiForgeryToken] public async Task<IActionResult> Create(
            [Bind("Id,BlogId,Title,Abstract,Content,Created,Updated")]
            Post post, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                post.CreatedDateTime = DateTime.UtcNow;
                if (image != null)
                {
                    post.FileName = image.FileName;
                    var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    post.Image = ms.ToArray();
                    ms.Close();
                    await ms.DisposeAsync();
                }
                else
                {
                    this.context.Add(post);
                    await this.context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewData["BlogId"] = new SelectList(this.context.Blog, "Id", "Id", post.BlogId);
            }

            return View(post);
        }


        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await this.context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = new SelectList(this.context.Blog, "Id", "Id", post.BlogId);
            return View(post);
        }


        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost] [ValidateAntiForgeryToken] public async Task<IActionResult> Edit(
            int id, [Bind("Id,BlogId,Title,Abstract,Content,Image,Created,Updated")]
            Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try

                {
                    this.context.Update(post);
                    await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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

            ViewData["BlogId"] = new SelectList(this.context.Blog, "Id", "Id", post.BlogId);
            return View(post);
        }


        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await this.context.Post
                                 .Include(p => p.Blog)
                                 .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }


        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")] [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await this.context.Post.FindAsync(id);
            this.context.Post.Remove(post);
            await this.context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return this.context.Post.Any(e => e.Id == id);
        }
    }
}