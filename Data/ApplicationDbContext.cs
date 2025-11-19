using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrabajoFinal.Models;

namespace TrabajoFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // TODO: Definir DbSets para las entidades
        public DbSet<Project> Projects { get; set; }
        public DbSet<FaqItem> FaqItems { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Summary).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Projects)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(e => e.Tags)
                    .HasMaxLength(int.MaxValue);
            });

            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(pm => pm.Id);
                entity.Property(pm => pm.Role).HasMaxLength(100);
                entity.HasOne(pm => pm.User)
                    .WithMany(u => u.ProjectMemberships)
                    .HasForeignKey(pm => pm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(pm => pm.Project)
                    .WithMany(p => p.Members)
                    .HasForeignKey(pm => pm.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(pm => new { pm.ProjectId, pm.UserId }).IsUnique();
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Description).HasMaxLength(300);
            });

            modelBuilder.Entity<UserSkill>(entity =>
            {
                entity.HasKey(us => us.Id);
                entity.HasOne(us => us.User)
                    .WithMany(u => u.UserSkills)
                    .HasForeignKey(us => us.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(us => us.Skill)
                    .WithMany(s => s.UserSkills)
                    .HasForeignKey(us => us.SkillId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(us => new { us.UserId, us.SkillId }).IsUnique();
            });

            modelBuilder.Entity<FaqItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Question).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Answer).IsRequired();
            });
        }
    }
}