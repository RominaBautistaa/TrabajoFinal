using System.ComponentModel.DataAnnotations;

namespace TrabajoFinal.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Summary { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        public string Status { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign key to ApplicationUser
        public string UserId { get; set; } = string.Empty;
        
        // Navigation property
        public virtual ApplicationUser? User { get; set; }

        // Collaboration members
        public virtual ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    }
}