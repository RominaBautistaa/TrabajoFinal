using TrabajoFinal.Models;

namespace TrabajoFinal.Services
{
    public class InMemoryProjectService : IProjectService
    {
        private readonly List<Project> _projects;

        public InMemoryProjectService()
        {
            // Datos de ejemplo con 12 proyectos
            _projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Title = "Sistema de Gestión de Biblioteca",
                    Summary = "Aplicación web para gestionar préstamos de libros y recursos bibliotecarios.",
                    Description = "Una solución completa desarrollada en ASP.NET Core que permite a las bibliotecas gestionar su inventario de libros, controlar préstamos y devoluciones, y mantener un registro de usuarios. Incluye funcionalidades de búsqueda avanzada, generación de reportes y notificaciones automáticas.",
                    Category = "Web",
                    Status = "Completado",
                    Tags = "ASP.NET, C#, SQL Server",
                    UpdatedAt = DateTime.Now.AddDays(-15)
                },
                new Project
                {
                    Id = 2,
                    Title = "App Móvil de Ejercicios",
                    Summary = "Aplicación para seguimiento de rutinas de ejercicio y nutrición.",
                    Description = "Aplicación móvil desarrollada en React Native que ayuda a los usuarios a crear y seguir rutinas de ejercicio personalizadas. Incluye contador de calorías, seguimiento de progreso con gráficos, y integración con dispositivos wearables para monitoreo en tiempo real.",
                    Category = "Móvil",
                    Status = "En desarrollo",
                    Tags = "React Native, JavaScript, Firebase",
                    UpdatedAt = DateTime.Now.AddDays(-3)
                },
                new Project
                {
                    Id = 3,
                    Title = "Análisis de Sentimientos en Redes Sociales",
                    Summary = "Herramienta de IA para analizar sentimientos en publicaciones de redes sociales.",
                    Description = "Sistema de análisis de datos que utiliza procesamiento de lenguaje natural para evaluar el sentimiento de publicaciones en Twitter y otras redes sociales. Implementa algoritmos de machine learning para clasificar emociones y generar insights sobre tendencias de opinión pública.",
                    Category = "IA",
                    Status = "Completado",
                    Tags = "Python, NLP, Machine Learning",
                    UpdatedAt = DateTime.Now.AddDays(-8)
                },
                new Project
                {
                    Id = 4,
                    Title = "Dashboard de Ventas Interactivo",
                    Summary = "Panel de control con visualizaciones dinámicas para análisis de ventas.",
                    Description = "Dashboard interactivo desarrollado con React y D3.js que permite visualizar datos de ventas en tiempo real. Incluye gráficos interactivos, filtros personalizables, exportación de reportes y integración con APIs de sistemas de ventas existentes.",
                    Category = "Datos",
                    Status = "Completado",
                    Tags = "React, D3.js, API REST",
                    UpdatedAt = DateTime.Now.AddDays(-12)
                },
                new Project
                {
                    Id = 5,
                    Title = "E-commerce de Productos Artesanales",
                    Summary = "Plataforma de comercio electrónico para artesanos locales.",
                    Description = "Marketplace online desarrollado con Node.js y MongoDB que conecta artesanos locales con compradores. Incluye sistema de pagos integrado, gestión de inventario, reseñas de productos, y herramientas de marketing digital para vendedores.",
                    Category = "Web",
                    Status = "En desarrollo",
                    Tags = "Node.js, MongoDB, Stripe",
                    UpdatedAt = DateTime.Now.AddDays(-5)
                },
                new Project
                {
                    Id = 6,
                    Title = "App de Realidad Aumentada para Educación",
                    Summary = "Aplicación móvil que utiliza AR para enseñanza interactiva.",
                    Description = "Aplicación educativa que utiliza realidad aumentada para hacer el aprendizaje más interactivo y visual. Los estudiantes pueden explorar modelos 3D de anatomía, química y física directamente desde su dispositivo móvil, con contenido adaptado a diferentes niveles educativos.",
                    Category = "Móvil",
                    Status = "Prototipo",
                    Tags = "Unity, ARCore, C#",
                    UpdatedAt = DateTime.Now.AddDays(-20)
                },
                new Project
                {
                    Id = 7,
                    Title = "Predictor de Precios de Criptomonedas",
                    Summary = "Modelo de IA para predecir fluctuaciones en el mercado crypto.",
                    Description = "Sistema de machine learning que analiza datos históricos y tendencias del mercado para predecir movimientos de precios en criptomonedas. Utiliza redes neuronales y análisis técnico para generar señales de trading con interfaces web intuitivas para visualizar predicciones.",
                    Category = "IA",
                    Status = "En desarrollo",
                    Tags = "TensorFlow, Python, Blockchain",
                    UpdatedAt = DateTime.Now.AddDays(-7)
                },
                new Project
                {
                    Id = 8,
                    Title = "Sistema de Monitoreo IoT",
                    Summary = "Plataforma para monitorear sensores IoT en tiempo real.",
                    Description = "Sistema completo de Internet de las Cosas para monitorear sensores ambientales en tiempo real. Incluye dashboard web para visualización de datos, alertas automáticas, integración con dispositivos Arduino/Raspberry Pi, y almacenamiento de datos históricos para análisis de tendencias.",
                    Category = "Datos",
                    Status = "Completado",
                    Tags = "Arduino, React, WebSocket",
                    UpdatedAt = DateTime.Now.AddDays(-25)
                },
                new Project
                {
                    Id = 9,
                    Title = "Red Social para Estudiantes",
                    Summary = "Plataforma social diseñada específicamente para el ámbito académico.",
                    Description = "Red social desarrollada con Vue.js y Laravel enfocada en conectar estudiantes universitarios. Permite formar grupos de estudio, compartir recursos académicos, organizar eventos educativos, y facilitar la colaboración en proyectos académicos entre pares.",
                    Category = "Web",
                    Status = "Beta",
                    Tags = "Vue.js, Laravel, MySQL",
                    UpdatedAt = DateTime.Now.AddDays(-10)
                },
                new Project
                {
                    Id = 10,
                    Title = "App de Gestión Financiera Personal",
                    Summary = "Herramienta móvil para control de gastos y presupuestos.",
                    Description = "Aplicación móvil desarrollada en Flutter para ayudar a usuarios a gestionar sus finanzas personales. Incluye categorización automática de gastos, establecimiento de presupuestos, recordatorios de paguestos, y generación de reportes financieros con gráficos detallados.",
                    Category = "Móvil",
                    Status = "Completado",
                    Tags = "Flutter, Dart, SQLite",
                    UpdatedAt = DateTime.Now.AddDays(-18)
                },
                new Project
                {
                    Id = 11,
                    Title = "Chatbot con Procesamiento de Lenguaje Natural",
                    Summary = "Asistente virtual inteligente para atención al cliente.",
                    Description = "Chatbot avanzado desarrollado con técnicas de NLP que puede mantener conversaciones naturales y resolver consultas de clientes de forma automática. Integra APIs de OpenAI, maneja múltiples idiomas, y se entrena continuamente para mejorar sus respuestas.",
                    Category = "IA",
                    Status = "En desarrollo",
                    Tags = "OpenAI, Python, FastAPI",
                    UpdatedAt = DateTime.Now.AddDays(-6)
                },
                new Project
                {
                    Id = 12,
                    Title = "Visualizador de Algoritmos de Ordenamiento",
                    Summary = "Herramienta educativa interactiva para enseñar algoritmos.",
                    Description = "Aplicación web educativa que visualiza diferentes algoritmos de ordenamiento de forma interactiva. Los estudiantes pueden ver paso a paso cómo funcionan algoritmos como quicksort, mergesort, bubblesort, etc., con animaciones, comparativas de performance y explicaciones teóricas integradas.",
                    Category = "Datos",
                    Status = "Completado",
                    Tags = "JavaScript, Canvas, Algoritmos",
                    UpdatedAt = DateTime.Now.AddDays(-30)
                }
            };
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _projects.OrderByDescending(p => p.UpdatedAt);
        }

        public IEnumerable<Project> GetFeaturedProjects(int count = 3)
        {
            return _projects
                .Where(p => p.Status == "Completado")
                .OrderByDescending(p => p.UpdatedAt)
                .Take(count);
        }

        public IEnumerable<Project> GetFilteredProjects(string? query = null, string? category = null, string? tag = null)
        {
            var filteredProjects = _projects.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                filteredProjects = filteredProjects.Where(p =>
                    p.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Summary.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                filteredProjects = filteredProjects.Where(p =>
                    p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                filteredProjects = filteredProjects.Where(p =>
                    !string.IsNullOrEmpty(p.Tags) && 
                    p.Tags.Split(',').Select(t => t.Trim()).Contains(tag, StringComparer.OrdinalIgnoreCase));
            }

            return filteredProjects.OrderByDescending(p => p.UpdatedAt);
        }

        public Project? GetProjectById(int id)
        {
            return _projects.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Project> GetRelatedProjects(int projectId, int count = 3)
        {
            var currentProject = GetProjectById(projectId);
            if (currentProject == null) return new List<Project>();

            return _projects
                .Where(p => p.Id != projectId && p.Category == currentProject.Category)
                .OrderByDescending(p => p.UpdatedAt)
                .Take(count);
        }

        public IEnumerable<string> GetAllCategories()
        {
            return _projects.Select(p => p.Category).Distinct().OrderBy(c => c);
        }

        public IEnumerable<string> GetAllTags()
        {
            return _projects
                .Where(p => !string.IsNullOrEmpty(p.Tags))
                .SelectMany(p => p.Tags.Split(',').Select(t => t.Trim()))
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .OrderBy(t => t);
        }
    }
}