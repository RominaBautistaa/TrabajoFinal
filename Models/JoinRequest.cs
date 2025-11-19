using System;
using System.ComponentModel.DataAnnotations;

namespace TrabajoFinal.Models
{
    public class JoinRequest
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(50)]
        [Display(Name = "Teléfono o WhatsApp")]
        public string Phone { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime DateSubmitted { get; set; } = DateTime.Now;
        
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}