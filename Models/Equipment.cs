using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipShare.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerDay { get; set; }

        public int CategoryId { get; set; }

        public int OwnerId { get; set; }

        [MaxLength(500)]
        public string Location { get; set; }

        public bool IsAvailable { get; set; } = true;

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual User Owner { get; set; }
    }
}