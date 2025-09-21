using EquipShare.Models.ViewModels;
using EquipShare.Services;
using EquipShare.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace EquipShare.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _equipmentService;
        private readonly IBookingService _bookingService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EquipmentController(IEquipmentService equipmentService, IBookingService bookingService, IWebHostEnvironment hostingEnvironment)
        {
            _equipmentService = equipmentService;
            _bookingService = bookingService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(string search, int? categoryId)
        {
            var equipment = string.IsNullOrEmpty(search) && !categoryId.HasValue
                ? _equipmentService.GetAllEquipment()
                : _equipmentService.SearchEquipment(search, categoryId);

            ViewBag.SearchQuery = search;
            ViewBag.CategoryId = categoryId;
            return View(equipment);
        }

        public IActionResult Details(int id)
        {
            var equipment = _equipmentService.GetEquipmentById(id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EquipmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imagePath = null;
                    if (model.ImageFile != null)
                    {
                        imagePath = ImageUploader.UploadImage(model.ImageFile, _hostingEnvironment.WebRootPath);
                    }

                    var userId = HttpContext.Session.GetInt32("UserId");
                    if (!userId.HasValue)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    var equipment = _equipmentService.CreateEquipment(model, userId.Value, imagePath);
                    return RedirectToAction("MyEquipment");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        public IActionResult MyEquipment()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var equipment = _equipmentService.GetEquipmentByOwner(userId.Value);

            // Get pending booking requests count
            var pendingRequestsCount = _bookingService.GetOwnerBookings(userId.Value)
                .Count(b => b.Status == "Pending");

            ViewBag.PendingRequestsCount = pendingRequestsCount;
            return View(equipment);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var equipment = _equipmentService.GetEquipmentById(id);
            if (equipment == null)
            {
                return NotFound();
            }

            // Check if the current user is the owner
            if (equipment.OwnerId != userId.Value)
            {
                return Forbid();
            }

            var model = new EquipmentViewModel
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Description = equipment.Description,
                PricePerDay = equipment.PricePerDay,
                CategoryId = equipment.CategoryId,
                Location = equipment.Location,
                ImageUrl = equipment.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EquipmentViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var equipment = _equipmentService.GetEquipmentById(id);
            if (equipment == null)
            {
                return NotFound();
            }

            // Check if the current user is the owner
            if (equipment.OwnerId != userId.Value)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imagePath = equipment.ImageUrl; // Keep existing image by default
                    if (model.ImageFile != null)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(equipment.ImageUrl))
                        {
                            ImageUploader.DeleteImage(equipment.ImageUrl, _hostingEnvironment.WebRootPath);
                        }
                        // Upload new image
                        imagePath = ImageUploader.UploadImage(model.ImageFile, _hostingEnvironment.WebRootPath);
                    }

                    var updatedEquipment = _equipmentService.UpdateEquipment(model, userId.Value, imagePath);
                    if (updatedEquipment != null)
                    {
                        TempData["SuccessMessage"] = "Equipment updated successfully!";
                        return RedirectToAction("MyEquipment");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update equipment.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var equipment = _equipmentService.GetEquipmentById(id);
            if (equipment == null)
            {
                return NotFound();
            }

            // Check if the current user is the owner
            if (equipment.OwnerId != userId.Value)
            {
                return Forbid();
            }

            return View(equipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, string confirmDelete)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var equipment = _equipmentService.GetEquipmentById(id);
            if (equipment == null)
            {
                return NotFound();
            }

            // Check if the current user is the owner
            if (equipment.OwnerId != userId.Value)
            {
                return Forbid();
            }

            if (confirmDelete == "Yes")
            {
                try
                {
                    // Delete image file if exists
                    if (!string.IsNullOrEmpty(equipment.ImageUrl))
                    {
                        ImageUploader.DeleteImage(equipment.ImageUrl, _hostingEnvironment.WebRootPath);
                    }

                    var result = _equipmentService.DeleteEquipment(id, userId.Value);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Equipment deleted successfully!";
                        return RedirectToAction("MyEquipment");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete equipment.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error deleting equipment: {ex.Message}";
                }
            }

            return RedirectToAction("MyEquipment");
        }
    }
}