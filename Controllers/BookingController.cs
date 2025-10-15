using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using EquipShare.Models.ViewModels;
using EquipShare.Services;

namespace EquipShare.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IEquipmentService _equipmentService;

        public BookingController(IBookingService bookingService, IEquipmentService equipmentService)
        {
            _bookingService = bookingService;
            _equipmentService = equipmentService;
        }

        [HttpGet]
        public IActionResult Create(int equipmentId, DateTime? startDate, DateTime? endDate, int? selectedBookingType, DateTime? oneDayBookingDate)
        {
            // Debug logging
            Console.WriteLine($"BookingController.Create called with equipmentId: {equipmentId}, selectedBookingType: {selectedBookingType}");

            var equipment = _equipmentService.GetEquipmentById(equipmentId);
            if (equipment == null)
            {
                Console.WriteLine($"Equipment with ID {equipmentId} not found");
                return NotFound();
            }

            // Check if user is trying to book their own equipment
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue && equipment.OwnerId == userId.Value)
            {
                TempData["ErrorMessage"] = "You cannot book your own equipment.";
                return RedirectToAction("Details", "Equipment", new { id = equipmentId });
            }

            var model = new BookingCreateModel
            {
                EquipmentId = equipmentId
            };

            // Check if we have preserved form data from a previous POST (validation errors)
            if (TempData["PreservedBookingType"] != null)
            {
                model.SelectedBookingType = (BookingType)TempData["PreservedBookingType"];
                model.OneDayBookingDate = TempData["PreservedOneDayBookingDate"] as DateTime?;
                model.StartDate = TempData["PreservedStartDate"] as DateTime?;
                model.EndDate = TempData["PreservedEndDate"] as DateTime?;
            }
            else
            {
                // Use parameters from equipment details page or defaults
                if (selectedBookingType.HasValue)
                {
                    model.SelectedBookingType = (BookingType)selectedBookingType.Value;

                    if (model.SelectedBookingType == BookingType.OneDay && oneDayBookingDate.HasValue)
                    {
                        model.OneDayBookingDate = oneDayBookingDate.Value;
                        model.StartDate = oneDayBookingDate.Value;
                        model.EndDate = oneDayBookingDate.Value.AddDays(1).AddTicks(-1);
                    }
                    else if (model.SelectedBookingType == BookingType.MultipleDay)
                    {
                        model.StartDate = startDate ?? DateTime.Today;
                        model.EndDate = endDate ?? (startDate ?? DateTime.Today).AddDays(1).AddTicks(-1);
                    }
                }
                else
                {
                    // Default values when no parameters provided
                    model.SelectedBookingType = BookingType.OneDay;
                    model.OneDayBookingDate = startDate ?? DateTime.Today;
                    model.StartDate = startDate ?? DateTime.Today;
                    model.EndDate = (startDate ?? DateTime.Today).AddDays(1).AddTicks(-1);
                }
            }

            // Calculate initial total and price breakdown (only if dates are not null)
            if (model.StartDate.HasValue && model.EndDate.HasValue)
            {
                var priceBreakdown = _bookingService.CalculatePriceBreakdown(equipmentId, model.StartDate.Value, model.EndDate.Value);
                model.TotalPrice = priceBreakdown.TotalPrice;
                model.EquipmentCost = priceBreakdown.EquipmentCost;
                model.PlatformCost = priceBreakdown.PlatformCost;
                model.OwnerReceivableAmount = priceBreakdown.OwnerReceivableAmount;
            }

            ViewBag.EquipmentName = equipment.Name;
            ViewBag.PricePerDay = equipment.PricePerDay;
            ViewBag.EquipmentImage = equipment.ImageUrl;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookingCreateModel model)
        {
            // Debug logging
            Console.WriteLine($"BookingController.Create POST called with EquipmentId: {model.EquipmentId}");

            if (ModelState.IsValid)
            {
                // Clear preserved data on successful validation
                TempData.Remove("PreservedBookingType");
                TempData.Remove("PreservedOneDayBookingDate");
                TempData.Remove("PreservedStartDate");
                TempData.Remove("PreservedEndDate");

                // Validate based on booking type
                DateTime startDate, endDate;
                if (model.SelectedBookingType == BookingType.OneDay)
                {
                    if (!model.OneDayBookingDate.HasValue)
                    {
                        ModelState.AddModelError("", "Please select a booking date.");
                        return View(model);
                    }
                    startDate = model.OneDayBookingDate.Value;
                    endDate = model.OneDayBookingDate.Value.AddDays(1).AddTicks(-1);
                }
                else // Multiple days
                {
                    if (!model.StartDate.HasValue || !model.EndDate.HasValue)
                    {
                        ModelState.AddModelError("", "Please select both start and end dates.");
                        return View(model);
                    }
                    startDate = model.StartDate.Value;
                    endDate = model.EndDate.Value;
                }

                // Check if equipment is available for the selected dates
                if (!_bookingService.IsEquipmentAvailable(model.EquipmentId, startDate, endDate))
                {
                    ModelState.AddModelError("", "This equipment is not available for the selected dates.");

                    // Preserve user's selections in TempData
                    PreserveFormData(model);

                    // Reload equipment details for the view
                    var equipment = _equipmentService.GetEquipmentById(model.EquipmentId);
                    ViewBag.EquipmentName = equipment.Name;
                    ViewBag.PricePerDay = equipment.PricePerDay;
                    ViewBag.EquipmentImage = equipment.ImageUrl;

                    return View(model);
                }

                // Calculate total price and price breakdown
                var priceBreakdown = _bookingService.CalculatePriceBreakdown(model.EquipmentId, startDate, endDate);
                model.TotalPrice = priceBreakdown.TotalPrice;
                model.EquipmentCost = priceBreakdown.EquipmentCost;
                model.PlatformCost = priceBreakdown.PlatformCost;
                model.OwnerReceivableAmount = priceBreakdown.OwnerReceivableAmount;

                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Account");
                }

                try
                {
                    var booking = _bookingService.CreateBooking(model, userId.Value);
                    return RedirectToAction("Details", new { id = booking.Id });
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);

                    // Preserve user's selections in TempData
                    PreserveFormData(model);

                    // Reload equipment details for the view
                    var equipment = _equipmentService.GetEquipmentById(model.EquipmentId);
                    if (equipment != null)
                    {
                        ViewBag.EquipmentName = equipment.Name;
                        ViewBag.PricePerDay = equipment.PricePerDay;
                        ViewBag.EquipmentImage = equipment.ImageUrl;
                    }
                    else
                    {
                        // Set default values when equipment is not found
                        ViewBag.EquipmentName = "Equipment Not Found";
                        ViewBag.PricePerDay = 0;
                        ViewBag.EquipmentImage = null;
                    }

                    return View(model);
                }
            }

            // If ModelState is invalid, preserve the form data
            PreserveFormData(model);
            return View(model);
        }

        private void PreserveFormData(BookingCreateModel model)
        {
            TempData["PreservedBookingType"] = (int)model.SelectedBookingType;
            TempData["PreservedOneDayBookingDate"] = model.OneDayBookingDate;
            TempData["PreservedStartDate"] = model.StartDate;
            TempData["PreservedEndDate"] = model.EndDate;
        }

        public IActionResult Details(int id)
        {
            var booking = _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Check if user has access to view this booking
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue || (booking.RenterId != userId.Value && booking.Equipment.OwnerId != userId.Value))
            {
                return Forbid();
            }

            return View(booking);
        }

        public IActionResult MyBookings()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var bookings = _bookingService.GetUserBookings(userId.Value);
            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int bookingId, string status)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Not authenticated" });
            }

            var success = _bookingService.UpdateBookingStatus(bookingId, status, userId.Value);
            return Json(new { success, message = success ? "Status updated successfully" : "Failed to update status" });
        }

        [HttpGet]
        public IActionResult GetBookingDates(int equipmentId)
        {
            var bookedDates = _bookingService.GetBookedDates(equipmentId);
            return Json(bookedDates);
        }

        public IActionResult MyRequests()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var bookings = _bookingService.GetOwnerBookings(userId.Value);
            return View(bookings);
        }
    }
}