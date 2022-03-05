using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;

namespace WebEnterprise.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext context;

        public PostController(ApplicationDbContext _context)
        {
            context = _context;
        }

        private void ViewCat()
        {
            var cats = context.Categories.Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();

            ViewBag.Cat = new SelectList(cats);
        }
        
        private void SelectedCat(int id)
        {
            var selectedCat = context.Categories.FirstOrDefault(c => c.Id == id);
            var cats = context.Categories.Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();
            ViewBag.SelectedCat = new SelectList(cats, selectedCat);
        }

        public IActionResult Index()
        {
            var posts = (from p in context.Posts
                         join c in context.Categories on p.CateId equals c.Id
                         join u in context.Users on p.UserId equals u.Id
                         select new PostDTO
                         {
                             Id = p.Id,
                             Title = p.Title,
                             Description = p.Description,
                             CatName = c.Name,
                             AuthorName = u.FullName,
                         }).ToList();
            ViewCat();
            return View(posts);
        }

        public IActionResult GetUserPost()
        {
            var posts = (from p in context.Posts
                         where p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)
                         select new Post
                         {
                             Id = p.Id,
                             Title = p.Title,
                             Description= p.Description,
                             CateId = p.CateId,
                             OpenDate = p.OpenDate,
                             ClosedDate = p.ClosedDate,
                         }).ToList();
            return View(posts);
        }

        [HttpPost]
        public IActionResult AddPost(PostDTO res)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = res.Title,
                    Description = res.Description,
                    CateId = res.CatId,
                    OpenDate = DateTime.Now,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };
                context.Posts.Add(post);
                context.SaveChanges();
                TempData["message"] = $"Successfully Add new Post {post.Title}";
                return RedirectToAction("Index");
            }
            return View(res);
        }

        public IActionResult EditPost(int id)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return RedirectToAction("Index");
            }
            SelectedCat(id);
            return View(post);
        }

        [HttpPost]
        public IActionResult EditPost(PostDTO res)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Id = res.Id,
                    Title = res.Title,
                    Description = res.Description,
                    OpenDate = res.OpenDate,
                    ClosedDate = res.ClosedDate,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    CateId = res.CatId,
                };
                context.Entry(post).State = EntityState.Modified;
                context.SaveChanges();
                TempData["message"] = $"Successfully Edit Post {post.Title}";
                return RedirectToAction("Index");
            }
            SelectedCat(res.Id);
            return View(res);
        }

        public IActionResult DeletePost(int id)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            if(post != null)
            {
                context.Remove(post);
                context.SaveChanges();
                TempData["message"] = $"Successfully Delete Post {post.Title}";
            }
            return RedirectToAction("Index");
        }
    }
}
