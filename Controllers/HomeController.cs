using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinal.Models;
using TrabajoFinal.Data;

namespace TrabajoFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Iniciando carga de proyectos para Home page");
                
                var totalProjects = await _context.Projects.CountAsync();
                _logger.LogInformation($"Total projects in database: {totalProjects}");
                
                if (totalProjects == 0)
                {
                    _logger.LogWarning("No hay proyectos en la tabla Projects");
                    
                    var emptyModel = new HomeViewModel
                    {
                        FeaturedProjects = new List<Project>(),
                        HeroTitle = "Descubre Proyectos Estudiantiles Increíbles",
                        HeroSubtitle = "Explora una colección de proyectos creativos e innovadores creados por estudiantes talentosos."
                    };

                    return View(emptyModel);
                }
                else
                {
                    var allProjects = await _context.Projects
                        .Include(p => p.User)
                        .ToListAsync();
                    _logger.LogInformation($"Proyectos obtenidos: {allProjects.Count}");

                    foreach (var project in allProjects.Take(5))
                    {
                        _logger.LogInformation($"Proyecto ID: {project.Id}, Title: {project.Title}, Category: {project.Category}, Status: {project.Status}");
                        _logger.LogInformation($"Tags: {(project.Tags != null && !string.IsNullOrEmpty(project.Tags) ? project.Tags : "NULL")}");
                    }

                    var featuredProjects = await _context.Projects
                        .Include(p => p.User)
                        .OrderByDescending(p => p.UpdatedAt)
                        .Take(3)
                        .ToListAsync();

                    _logger.LogInformation($"Featured projects count: {featuredProjects.Count}");

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
                    if (User.Identity.IsAuthenticated)
                    {
                        var currentUserId = _userManager.GetUserId(User);
                        var userProjectMembers = await _context.ProjectMembers
                            .Where(pm => pm.UserId == currentUserId)
                            .Select(pm => pm.ProjectId)
                            .ToListAsync();

                        foreach (var projectId in userProjectMembers)
                        {
                            projectMemberList[projectId] = true;
                        }
                    }
                    ViewBag.ProjectMemberList = projectMemberList;

                    // Obtener solicitudes del usuario actual
                    var userJoinRequests = new Dictionary<int, string>();
                    if (User.Identity.IsAuthenticated)
                    {
                        var currentUserId = _userManager.GetUserId(User);
                        var userRequests = await _context.JoinRequests
                            .Where(jr => jr.UserId == currentUserId)
                            .Select(jr => new { jr.ProjectId, jr.Status })
                            .ToListAsync();

                        foreach (var request in userRequests)
                        {
                            userJoinRequests[request.ProjectId] = request.Status;
                        }
                    }
                    ViewBag.UserJoinRequests = userJoinRequests;

                    var model = new HomeViewModel
                    {
                        FeaturedProjects = featuredProjects,
                        HeroTitle = "Descubre Proyectos Estudiantiles Increíbles",
                        HeroSubtitle = "Explora una colección de proyectos creativos e innovadores creados por estudiantes talentosos."
                    };

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading projects from database");
                var model = new HomeViewModel
                {
                    FeaturedProjects = new List<Project>(),
                    HeroTitle = "Descubre Proyectos Estudiantiles Increíbles",
                    HeroSubtitle = "Explora una colección de proyectos creativos e innovadores creados por estudiantes talentosos."
                };

                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
