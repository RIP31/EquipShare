using System;
using System.ComponentModel.DataAnnotations;

namespace EquipShare.Models.ViewModels
{
    public class BookingViewModel
    {
        public int EquipmentId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

        [Display(Name = "Total Days")]
        public int TotalDays { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        // Equipment details for display
        public string EquipmentName { get; set; }
        public decimal PricePerDay { get; set; }
        public string EquipmentImage { get; set; }
    }
}