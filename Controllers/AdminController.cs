using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HealthForumMVC.Data;
using HealthForumMVC.Models;

namespace HealthForumMVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != UserRole.Admin)
            {
                return Forbid();
            }

            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalPosts = await _context.Posts.CountAsync(),
                TotalComments = await _context.Comments.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                RecentPosts = await _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.Category)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                RecentUsers = await _context.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(stats);
        }

        public async Task<IActionResult> Posts()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != UserRole.Admin) return Forbid();

            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(posts);
        }

        public async Task<IActionResult> Users()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != UserRole.Admin) return Forbid();

            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != UserRole.Admin) return Forbid();

            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Posts");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePin(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != UserRole.Admin) return Forbid();

            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.IsPinned = !post.IsPinned;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Posts");
        }
    }
}