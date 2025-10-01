using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EquipShare.Models.ViewModels
{
    public class EquipmentViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Equipment Name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price Per Day")]
        [Range(0.01, 10000)]
        public decimal PricePerDay { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Location")]
        [MaxLength(500)]
        public string Location { get; set; }

        [Display(Name = "Equipment Images")]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

        public string ImageUrl { get; set; }

        // Helper property to check if new images are being uploaded
        public bool HasNewImages => ImageFiles != null && ImageFiles.Any(f => f != null);
    }
}