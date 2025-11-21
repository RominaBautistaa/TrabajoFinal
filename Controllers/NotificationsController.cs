using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinal.Data;
using TrabajoFinal.Models;

namespace TrabajoFinal.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            
            // Get join request notifications (requests sent by this user)
            var myRequests = await _context.JoinRequests
                .Include(jr => jr.Project)
                .Where(jr => jr.UserId == userId)
                .OrderByDescending(jr => jr.DateSubmitted)
                .ToListAsync();

            // Get notifications when someone joins a project I created
            var myProjects = await _context.Projects
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .ToListAsync();
                
            var memberRequestsForMyProjects = await _context.ProjectMembers
                .Include(pm => pm.User)
                .Include(pm => pm.Project)
                .Where(pm => myProjects.Contains(pm.ProjectId))
                .OrderByDescending(pm => pm.JoinedAt)
                .Take(10) // Get last 10 members
                .ToListAsync();

            var viewModel = new NotificationsViewModel
            {
                JoinRequestNotifications = myRequests,
                ProjectMemberNotifications = memberRequestsForMyProjects
            };

            return View(viewModel);
        }
    }

    public class NotificationsViewModel
    {
        public IEnumerable<JoinRequest> JoinRequestNotifications { get; set; } = new List<JoinRequest>();
        public IEnumerable<ProjectMember> ProjectMemberNotifications { get; set; } = new List<ProjectMember>();
    }
}