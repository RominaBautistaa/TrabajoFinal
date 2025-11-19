namespace TrabajoFinal.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Project> FeaturedProjects { get; set; } = new List<Project>();
        public string HeroTitle { get; set; } = "Transforma Ideas en Realidad";
        public string HeroSubtitle { get; set; } = "Explora proyectos estudiantiles innovadores que inspiran, crean y conectan.";
    }
}