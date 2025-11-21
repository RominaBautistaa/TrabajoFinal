using Microsoft.AspNetCore.Identity;

namespace TrabajoFinal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation property for projects created by this user
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        // Navigation: project memberships
        public virtual ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();

        // Navigation: user skills
        public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

        // Navigation: join requests made by this user
        public virtual ICollection<JoinRequest> JoinRequests { get; set; } = new List<JoinRequest>();
    }
}