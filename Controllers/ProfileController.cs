using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EquipShare.Services;
using EquipShare.Models;
using System.Linq;

namespace EquipShare.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAuthService _authService;

        public ProfileController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _authService.GetUserById(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}