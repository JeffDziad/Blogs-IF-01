using Microsoft.AspNetCore.Mvc;
using Blogs.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Blogs.Controllers
{
    public class HomeController : Controller
    {
        private BloggingContext _bloggingContext;

        public HomeController(BloggingContext db) => _bloggingContext = db;
        
        // home
        public IActionResult Index() => View(_bloggingContext.Blogs.OrderBy(b => b.Name));
        
        // home/AddBlog
        [Authorize(Roles = "blogs-moderate")]
        public IActionResult AddBlog() => View();
        
        // http post home/AddBlog - Add a blog
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "blogs-moderate")]
        public IActionResult AddBlog(Blog model)
        {
            if (ModelState.IsValid)
            {
                if (_bloggingContext.Blogs.Any(b => b.Name == model.Name))
                {
                    ModelState.AddModelError("", "Name must be unique");
                }
                else
                {
                    _bloggingContext.AddBlog(model);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        //http post home/DeleteBlog - delete a blog
        [Authorize(Roles = "blogs-moderate")]
        public IActionResult DeleteBlog(int id)
        {
            _bloggingContext.DeleteBlog(_bloggingContext.Blogs.FirstOrDefault(b => b.BlogId == id));
            return RedirectToAction("Index");
        }

        //Delete Post
        [Authorize(Roles = "blogs-moderate")]
        public IActionResult DeletePost(int id)
        {
            Post post = _bloggingContext.Posts.FirstOrDefault(p => p.PostId == id);
            int BlogId = post.BlogId;
            _bloggingContext.DeletePost(post);
            return RedirectToAction("BlogDetail", new { id = BlogId });
        }

        //Send data about a given Blog using id
        public IActionResult BlogDetail(int id) => View(new PostViewModel
        {
            blog = _bloggingContext.Blogs.FirstOrDefault(b => b.BlogId == id),
            Posts = _bloggingContext.Posts.Where(p => p.BlogId == id)
        });

        //Add Post to blog
        [Authorize]
        public IActionResult AddPost(int id)
        {
            ViewBag.BlogId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPost(int id, Post post)
        {
            post.BlogId = id;
            if (ModelState.IsValid)
            {
                _bloggingContext.AddPost(post);
                return RedirectToAction("BlogDetail", new { id = id });
            }
            @ViewBag.BlogId = id;
            return View();
        }

    }
}