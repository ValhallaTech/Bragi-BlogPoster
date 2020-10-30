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
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Post.Include(p => p.Blog);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
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
                var blog = _context.Blog.Find(id);
                if (blog == null)
                {
                    return NotFound();
                }

                post                 = new Post() {BlogId = (int) id};
                ViewData["BlogName"] = blog.Name;
            }
            else
            {
                ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Name");
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
                    _context.Add(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id", post.BlogId);
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

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id", post.BlogId);
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
                    _context.Update(post);
                    await _context.SaveChangesAsync();
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

            ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id", post.BlogId);
            return View(post);
        }


        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
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
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}