using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EquipShare.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}