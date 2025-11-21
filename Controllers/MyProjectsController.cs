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

            // Get projects where user is the creator and projects where user is a member
            var createdProjects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var memberProjects = await _context.ProjectMembers
                .Where(pm => pm.UserId == userId)
                .Select(pm => pm.Project)
                .ToListAsync();

            // Combine both lists and remove duplicates
            var allProjects = createdProjects
                .Concat(memberProjects)
                .Distinct()
                .OrderByDescending(p => p.UpdatedAt)
                .ToList();

            // Obtener conteo de miembros para cada proyecto
            var projectMemberCounts = new Dictionary<int, int>();
            var allProjectMemberCounts = await _context.ProjectMembers
                .GroupBy(pm => pm.ProjectId)
                .Select(g => new { ProjectId = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var count in allProjectMemberCounts)
            {
                projectMemberCounts[count.ProjectId] = count.Count;
            }
            ViewBag.ProjectMemberCounts = projectMemberCounts;

            // Obtener si el usuario actual es miembro de cada proyecto
            var projectMemberList = new Dictionary<int, bool>();
            var userProjectMembers = await _context.ProjectMembers
                .Where(pm => pm.UserId == userId)
                .Select(pm => pm.ProjectId)
                .ToListAsync();

            foreach (var projectId in userProjectMembers)
            {
                projectMemberList[projectId] = true;
            }
            ViewBag.ProjectMemberList = projectMemberList;

            // Obtener solicitudes del usuario actual
            var userJoinRequests = new Dictionary<int, string>();
            var allProjectIds = allProjects.Select(p => p.Id).ToList(); // Todos los proyectos que se están mostrando
            var userRequests = await _context.JoinRequests
                .Where(jr => jr.UserId == userId && allProjectIds.Contains(jr.ProjectId))
                .Select(jr => new { jr.ProjectId, jr.Status })
                .ToListAsync();

            foreach (var request in userRequests)
            {
                userJoinRequests[request.ProjectId] = request.Status;
            }
            ViewBag.UserJoinRequests = userJoinRequests;

            return View(allProjects);
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