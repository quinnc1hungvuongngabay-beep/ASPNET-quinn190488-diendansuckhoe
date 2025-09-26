using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using HealthForumMVC.Data;
using HealthForumMVC.Models;

namespace HealthForumMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToListAsync();

            var stats = new
            {
                MemberCount = await _context.Users.CountAsync(),
                PostCount = await _context.Posts.CountAsync(),
                OnlineCount = 234,
                ActivityRate = "89%"
            };

            ViewBag.Stats = stats;
            return View(posts);
        }

        public async Task<IActionResult> PostDetail(string id)
        {
            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

            post.ViewCount++;
            await _context.SaveChangesAsync();

            return View(post);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreatePost()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(Post post)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    post.Id = Guid.NewGuid().ToString();
                    post.AuthorId = user.Id;
                    post.CreatedAt = DateTime.UtcNow;
                    post.UpdatedAt = DateTime.UtcNow;

                    _context.Posts.Add(post);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("PostDetail", new { id = post.Id });
                }
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(string postId, string content)
        {
            if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(postId))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var comment = new Comment
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = content,
                        PostId = postId,
                        AuthorId = user.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Comments.Add(comment);

                    var post = await _context.Posts.FindAsync(postId);
                    if (post != null)
                    {
                        post.CommentCount++;
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("PostDetail", new { id = postId });
        }

        public async Task<IActionResult> Popular()
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .OrderByDescending(p => p.LikeCount)
                .ThenByDescending(p => p.ViewCount)
                .Take(10)
                .ToListAsync();

            var stats = new
            {
                MemberCount = await _context.Users.CountAsync(),
                PostCount = await _context.Posts.CountAsync(),
                OnlineCount = 234,
                ActivityRate = "89%"
            };

            ViewBag.Stats = stats;
            ViewBag.Filter = "popular";
            return View("Index", posts);
        }

        public async Task<IActionResult> Trending()
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Where(p => p.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                .OrderByDescending(p => p.ViewCount + p.LikeCount * 2)
                .Take(10)
                .ToListAsync();

            var stats = new
            {
                MemberCount = await _context.Users.CountAsync(),
                PostCount = await _context.Posts.CountAsync(),
                OnlineCount = 234,
                ActivityRate = "89%"
            };

            ViewBag.Stats = stats;
            ViewBag.Filter = "trending";
            return View("Index", posts);
        }

        public async Task<IActionResult> Category(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Where(p => p.CategoryId == id)
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToListAsync();

            var stats = new
            {
                MemberCount = await _context.Users.CountAsync(),
                PostCount = await _context.Posts.CountAsync(),
                OnlineCount = 234,
                ActivityRate = "89%"
            };

            ViewBag.Stats = stats;
            ViewBag.Category = category;
            return View("Index", posts);
        }

        public IActionResult Saved()
        {
            // Placeholder for saved posts functionality
            var posts = new List<Post>();
            
            var stats = new
            {
                MemberCount = 0,
                PostCount = 0,
                OnlineCount = 234,
                ActivityRate = "89%"
            };

            ViewBag.Stats = stats;
            ViewBag.Filter = "saved";
            return View("Index", posts);
        }
    }
}