using System.ComponentModel.DataAnnotations;

namespace TrabajoFinal.Models
{
    public class UserSkill
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int SkillId { get; set; }

        [Range(1, 5)]
        public int Proficiency { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual ApplicationUser? User { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}


