using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EquipShare.Data;

namespace EquipShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get featured equipment
            var featuredEquipment = _context.Equipment
                .Where(e => e.IsAvailable)
                .OrderByDescending(e => e.CreatedDate)
                .Take(6)
                .ToList();

            return View(featuredEquipment);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}