using System;
using System.ComponentModel.DataAnnotations;

namespace EquipShare.Models.ViewModels
{
    public class BookingCreateModel
    {
        public int EquipmentId { get; set; }

        [Required]
        [Display(Name = "Booking Type")]
        public BookingType SelectedBookingType { get; set; } = BookingType.OneDay;

        // For one-day booking
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateTime? OneDayBookingDate { get; set; }

        // For multiple-day booking
        [RequiredIf("SelectedBookingType", BookingType.MultipleDay)]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [RequiredIf("SelectedBookingType", BookingType.MultipleDay)]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Total Days")]
        public int TotalDays => CalculateTotalDays();

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        private int CalculateTotalDays()
        {
            if (SelectedBookingType == BookingType.OneDay && OneDayBookingDate.HasValue)
            {
                return 1;
            }
            else if (SelectedBookingType == BookingType.MultipleDay && StartDate.HasValue && EndDate.HasValue)
            {
                return (EndDate.Value.Date - StartDate.Value.Date).Days + 1;
            }
            return 0;
        }
    }

    public enum BookingType
    {
        OneDay = 1,
        MultipleDay = 2
    }

    // Custom validation attribute for conditional requirements
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object _desiredValue;

        public RequiredIfAttribute(string propertyName, object desiredValue)
        {
            _propertyName = propertyName;
            _desiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_propertyName);
            if (property == null)
                return new ValidationResult($"Property {_propertyName} not found.");

            var propertyValue = property.GetValue(validationContext.ObjectInstance);

            if (object.Equals(propertyValue, _desiredValue))
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required when {_propertyName} is {_desiredValue}.");
            }

            return ValidationResult.Success;
        }
    }
}