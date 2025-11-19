using TrabajoFinal.Models;

namespace TrabajoFinal.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> GetAllProjects();
        IEnumerable<Project> GetFeaturedProjects(int count = 3);
        IEnumerable<Project> GetFilteredProjects(string? query = null, string? category = null, string? tag = null);
        Project? GetProjectById(int id);
        IEnumerable<Project> GetRelatedProjects(int projectId, int count = 3);
        IEnumerable<string> GetAllCategories();
        IEnumerable<string> GetAllTags();
    }
}