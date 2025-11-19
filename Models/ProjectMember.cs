using System.ComponentModel.DataAnnotations;

namespace TrabajoFinal.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int ProjectId { get; set; }

        [MaxLength(100)]
        public string Role { get; set; } = "Member";

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual ApplicationUser? User { get; set; }
        public virtual Project? Project { get; set; }
    }
}


