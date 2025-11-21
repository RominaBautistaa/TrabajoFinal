using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using TrabajoFinal.Data;
using TrabajoFinal.Models;

namespace TrabajoFinal.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(ApplicationDbContext context, ILogger<ProjectsController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // Muestra todos los proyectos con filtro opcional
        public async Task<IActionResult> Index(string? q, string? category, string? tag, int page = 1)
        {
            try
            {
                _logger.LogInformation($"Iniciando carga de proyectos para Projects page. Filtros - q: {q}, category: {category}, tag: {tag}");

                // Obtener todos los proyectos desde la base de datos incluyendo la información de usuario
                var allProjects = await _context.Projects
                    .Include(p => p.User)
                    .ToListAsync(); // Cargamos todos los proyectos con su usuario
                _logger.LogInformation($"Total projects loaded from database: {allProjects.Count}");

                if (allProjects.Count > 0)
                {
                    // Mostrar información de los primeros 5 proyectos para diagnóstico
                    foreach (var project in allProjects.Take(5))
                    {
                        _logger.LogInformation($"Project ID: {project.Id}, Title: {project.Title}, Category: {project.Category}, Status: {project.Status}");
                        _logger.LogInformation($"Tags: {(project.Tags != null && !string.IsNullOrEmpty(project.Tags) ? project.Tags : "NULL")}");
                        _logger.LogInformation($"UpdatedAt: {project.UpdatedAt}");
                        _logger.LogInformation($"User: {(project.User != null ? project.User.UserName : "NULL")}");
                    }
                }

                // Aplicar filtros en memoria
                var filteredProjects = allProjects.AsEnumerable(); // Ahora trabajamos en memoria para manejar Tags

                if (!string.IsNullOrEmpty(q))
                    filteredProjects = filteredProjects.Where(p =>
                        p.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                        p.Description.Contains(q, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(category))
                    filteredProjects = filteredProjects.Where(p =>
                        string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(tag))
                    filteredProjects = filteredProjects.Where(p =>
                        !string.IsNullOrEmpty(p.Tags) &&
                        p.Tags.Split(',').Any(t =>
                            string.Equals(t.Trim(), tag, StringComparison.OrdinalIgnoreCase)));

                // Ordenar y convertir a lista
                var projects = filteredProjects
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

                // Obtener categorías
                ViewBag.Categories = allProjects
                    .Where(p => !string.IsNullOrEmpty(p.Category))
                    .Select(p => p.Category)
                    .Distinct()
                    .ToList();

                // Obtener tags (desde todos los proyectos)
                ViewBag.Tags = allProjects
                    .Where(p => !string.IsNullOrEmpty(p.Tags))
                    .SelectMany(p => p.Tags.Split(',').Select(t => t.Trim()))
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Distinct()
                    .OrderBy(t => t)
                    .ToList();

                ViewBag.HasResults = projects.Any();
                ViewBag.ResultsCount = projects.Count;

                _logger.LogInformation($"Projects after filtering: {projects.Count}, Categories: {ViewBag.Categories.Count}, Tags: {ViewBag.Tags.Count}");

                return View(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading projects from database in ProjectsController");
                // Return an empty list in case of error
                ViewBag.HasResults = false;
                ViewBag.ResultsCount = 0;
                ViewBag.Categories = new List<string>();
                ViewBag.Tags = new List<string>();
                ViewBag.ProjectMemberCounts = new Dictionary<int, int>();

                return View(new List<Project>());
            }
        }


        // Detalle de un proyecto específico
        public async Task<IActionResult> Detail(int id)
        {
            var project = await _context.Projects
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound();

            // Obtener conteo de miembros para este proyecto
            var memberCount = await _context.ProjectMembers
                .Where(pm => pm.ProjectId == id)
                .CountAsync();

            ViewBag.CurrentMemberCount = memberCount;

            // Proyectos relacionados (misma categoría)
            var allProjects = await _context.Projects
                .Include(p => p.User)
                .ToListAsync();
            var relatedProjects = allProjects
                .Where(p => p.Category == project.Category && p.Id != project.Id)
                .OrderByDescending(p => p.UpdatedAt)
                .Take(3)
                .ToList();

            // Obtener conteo de miembros para proyectos relacionados
            var relatedProjectIds = relatedProjects.Select(p => p.Id).ToList();
            var relatedProjectMemberCounts = new Dictionary<int, int>();
            if (relatedProjectIds.Any())
            {
                var relatedMemberCounts = await _context.ProjectMembers
                    .Where(pm => relatedProjectIds.Contains(pm.ProjectId))
                    .GroupBy(pm => pm.ProjectId)
                    .Select(g => new { ProjectId = g.Key, Count = g.Count() })
                    .ToListAsync();

                foreach (var count in relatedMemberCounts)
                {
                    relatedProjectMemberCounts[count.ProjectId] = count.Count;
                }
            }
            ViewBag.ProjectMemberCounts = relatedProjectMemberCounts;

            // Obtener si el usuario actual es miembro de cada proyecto relacionado
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

            // Obtener solicitudes del usuario actual para proyectos relacionados y todos los otros proyectos
            var userJoinRequests = new Dictionary<int, string>();
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = _userManager.GetUserId(User);
                var allProjectIds = allProjects.Select(p => p.Id).ToList(); // Todos los proyectos, no solo relacionados
                var userRequests = await _context.JoinRequests
                    .Where(jr => jr.UserId == currentUserId && allProjectIds.Contains(jr.ProjectId))
                    .Select(jr => new { jr.ProjectId, jr.Status })
                    .ToListAsync();

                foreach (var request in userRequests)
                {
                    userJoinRequests[request.ProjectId] = request.Status;
                }
            }
            ViewBag.UserJoinRequests = userJoinRequests;

            ViewBag.RelatedProjects = relatedProjects;

            return View(project);
        }

        // GET: Projects/Manage/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Manage(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound();

            // Verificar que el proyecto pertenece al usuario autenticado
            var currentUserId = _userManager.GetUserId(User);
            if (project.UserId != currentUserId)
                return Forbid();

            return View(project);
        }

        // POST: Projects/Manage/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(int id, Project model)
        {
            if (id != model.Id)
                return NotFound();
                
            // Verificar que el proyecto pertenece al usuario autenticado
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
                return NotFound();
                
            var currentUserId = _userManager.GetUserId(User);
            if (existingProject.UserId != currentUserId)
                return Forbid();
                
            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar solo las propiedades editables
                    existingProject.Title = model.Title;
                    existingProject.Summary = model.Summary;
                    existingProject.Description = model.Description;
                    existingProject.Category = model.Category;
                    existingProject.Status = model.Status;
                    existingProject.Tags = model.Tags;
                    existingProject.UpdatedAt = DateTime.UtcNow;
                    
                    _context.Update(existingProject);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Proyecto actualizado exitosamente.";
                    return RedirectToAction("Index", "MyProjects");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating project {ProjectId}", id);
                    ModelState.AddModelError("", "Hubo un error al actualizar el proyecto. Por favor, inténtalo de nuevo.");
                }
            }
            
            return View(existingProject);
        }

        // POST: Projects/Delete/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();
                
            // Verificar que el proyecto pertenece al usuario autenticado
            var currentUserId = _userManager.GetUserId(User);
            if (project.UserId != currentUserId)
                return Forbid();
                
            try
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Proyecto eliminado exitosamente.";
                return RedirectToAction("Index", "MyProjects");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId}", id);
                TempData["ErrorMessage"] = "Hubo un error al eliminar el proyecto. Por favor, inténtalo de nuevo.";
                return RedirectToAction("Index", "MyProjects");
            }
        }
    }
}
