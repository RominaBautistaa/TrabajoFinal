using System.ComponentModel.DataAnnotations;

namespace TrabajoFinal.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    }
}


