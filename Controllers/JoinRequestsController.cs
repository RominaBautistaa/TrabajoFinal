using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinal.Data;
using TrabajoFinal.Models;

namespace TrabajoFinal.Controllers
{
    [Authorize]
    public class JoinRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JoinRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JoinRequests/Create
        public async Task<IActionResult> Create(int projectId)
        {
            // Verificar que el proyecto existe
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            // Crear un nuevo objeto JoinRequest con el ProjectId predefinido
            var joinRequest = new JoinRequest
            {
                ProjectId = projectId
            };

            return View(joinRequest);
        }

        // POST: JoinRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] JoinRequest request)
        {
            Console.WriteLine($"Método POST Create llamado");
            
            if (request == null)
            {
                Console.WriteLine($"Request es null");
                return Json(new { success = false, message = "Datos inválidos." });
            }
            
            Console.WriteLine($"Request recibido: ProjectId={request.ProjectId}, Skills.Length={request?.Skills?.Length}, Message.Length={request?.Message?.Length}, Phone.Length={request?.Phone?.Length}");

            try
            {
                // Validar manualmente los campos requeridos
                if (request.ProjectId <= 0 || string.IsNullOrEmpty(request.Skills) || string.IsNullOrEmpty(request.Message) || string.IsNullOrEmpty(request.Phone))
                {
                    Console.WriteLine($"Validación fallida: ProjectId={request.ProjectId}, Skills.IsNullOrEmpty={string.IsNullOrEmpty(request.Skills)}, Message.IsNullOrEmpty={string.IsNullOrEmpty(request.Message)}, Phone.IsNullOrEmpty={string.IsNullOrEmpty(request.Phone)}");
                    return Json(new { success = false, message = "Por favor completa todos los campos antes de enviar." });
                }

                // Crear la solicitud de unión con los datos recibidos
                var joinRequest = new JoinRequest
                {
                    ProjectId = request.ProjectId,
                    Skills = request.Skills,
                    Message = request.Message,
                    Phone = request.Phone,
                    DateSubmitted = DateTime.Now,
                    DateCreated = DateTime.Now
                };

                // Asociar el UserId del usuario autenticado
                joinRequest.UserId = _userManager.GetUserId(User);
                
                Console.WriteLine($"Usuario autenticado: {joinRequest.UserId}");
                Console.WriteLine($"Intentando guardar solicitud - UserId: {joinRequest.UserId}, ProjectId: {joinRequest.ProjectId}, Skills Length: {joinRequest.Skills?.Length}");
                
                // Agregar a la base de datos
                _context.JoinRequests.Add(joinRequest);
                
                var entriesAdded = _context.ChangeTracker.Entries().Count(e => e.State == EntityState.Added);
                Console.WriteLine($"Entidades marcadas como Added antes de SaveChanges: {entriesAdded}");
                
                await _context.SaveChangesAsync();
                
                var entriesSaved = _context.ChangeTracker.Entries().Count();
                Console.WriteLine($"Entidades totales después de SaveChanges: {entriesSaved}");
                
                // Confirmar que se guardó el registro
                Console.WriteLine($"Solicitud guardada: ID {joinRequest.Id} (después de SaveChanges), Usuario: {joinRequest.UserId}, Proyecto: {joinRequest.ProjectId}");
                
                return Json(new { success = true, message = "Solicitud enviada correctamente." });
            }
            catch (Exception ex)
            {
                // En caso de error, devolver error como JSON con detalles específicos
                Console.WriteLine($"Error al guardar solicitud: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = $"No se pudo guardar la solicitud. Intenta nuevamente. Error: {ex.Message}" });
            }
        }

        // GET: JoinRequests/Success
        public IActionResult Success()
        {
            return View();
        }
        
        // GET: JoinRequests/List
        public async Task<IActionResult> List()
        {
            var joinRequests = await _context.JoinRequests.ToListAsync();
            return View(joinRequests);
        }
    }
}