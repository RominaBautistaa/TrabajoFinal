using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinal.Data;
using TrabajoFinal.Models;

namespace TrabajoFinal.Controllers
{
    [Authorize]
    public class MyProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyProjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MyProjects
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var userProjects = await _context.Projects
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.UpdatedAt)
                .ToListAsync();
            
            return View(userProjects);
        }

        // GET: MyProjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyProjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.UserId = _userManager.GetUserId(User);
                project.UpdatedAt = DateTime.UtcNow;
                _context.Add(project);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Proyecto creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(project);
        }
    }
}