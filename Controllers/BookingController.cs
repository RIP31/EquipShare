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
        public IActionResult Create(int equipmentId, DateTime? startDate, DateTime? endDate)
        {
            // Debug logging
            Console.WriteLine($"BookingController.Create called with equipmentId: {equipmentId}");

            var equipment = _equipmentService.GetEquipmentById(equipmentId);
            if (equipment == null)
            {
                Console.WriteLine($"Equipment with ID {equipmentId} not found");
                return NotFound();
            }

            var model = new BookingCreateModel
            {
                EquipmentId = equipmentId,
                SelectedBookingType = BookingType.OneDay,
                OneDayBookingDate = startDate ?? DateTime.Today,
                StartDate = startDate ?? DateTime.Today,
                EndDate = (startDate ?? DateTime.Today).AddDays(1).AddTicks(-1) // End of the selected day
            };

            // Calculate initial total
            model.TotalPrice = _bookingService.CalculateTotalPrice(equipmentId, model.StartDate.Value, model.EndDate.Value);

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

                    // Reload equipment details for the view
                    var equipment = _equipmentService.GetEquipmentById(model.EquipmentId);
                    ViewBag.EquipmentName = equipment.Name;
                    ViewBag.PricePerDay = equipment.PricePerDay;
                    ViewBag.EquipmentImage = equipment.ImageUrl;

                    return View(model);
                }

                // Calculate total price
                model.TotalPrice = _bookingService.CalculateTotalPrice(model.EquipmentId, startDate, endDate);

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

            return View(model);
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