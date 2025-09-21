using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EquipShare.Data;
using EquipShare.Models;
using EquipShare.Models.ViewModels;

namespace EquipShare.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly ApplicationDbContext _context;

        public EquipmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Equipment> GetAllEquipment()
        {
            return _context.Equipment
                .Include(e => e.Category)
                .Include(e => e.Owner)
                .Where(e => e.IsAvailable)
                .OrderByDescending(e => e.CreatedDate)
                .ToList();
        }

        public List<Equipment> GetEquipmentByOwner(int ownerId)
        {
            return _context.Equipment
                .Include(e => e.Category)
                .Where(e => e.OwnerId == ownerId)
                .OrderByDescending(e => e.CreatedDate)
                .ToList();
        }

        public Equipment GetEquipmentById(int id)
        {
            return _context.Equipment
                .Include(e => e.Category)
                .Include(e => e.Owner)
                .FirstOrDefault(e => e.Id == id);
        }

        public Equipment CreateEquipment(EquipmentViewModel model, int ownerId, string imagePath)
        {
            var equipment = new Equipment
            {
                Name = model.Name,
                Description = model.Description,
                PricePerDay = model.PricePerDay,
                CategoryId = model.CategoryId,
                OwnerId = ownerId,
                Location = model.Location,
                ImageUrl = imagePath,
                IsAvailable = true
            };

            _context.Equipment.Add(equipment);
            _context.SaveChanges();

            return equipment;
        }

        public Equipment UpdateEquipment(EquipmentViewModel model, int ownerId, string imagePath)
        {
            var equipment = _context.Equipment.FirstOrDefault(e => e.Id == model.Id && e.OwnerId == ownerId);

            if (equipment == null)
                return null;

            equipment.Name = model.Name;
            equipment.Description = model.Description;
            equipment.PricePerDay = model.PricePerDay;
            equipment.CategoryId = model.CategoryId;
            equipment.Location = model.Location;

            if (!string.IsNullOrEmpty(imagePath))
            {
                equipment.ImageUrl = imagePath;
            }

            _context.Equipment.Update(equipment);
            _context.SaveChanges();

            return equipment;
        }

        public bool DeleteEquipment(int id, int ownerId)
        {
            var equipment = _context.Equipment.FirstOrDefault(e => e.Id == id && e.OwnerId == ownerId);

            if (equipment == null)
                return false;

            _context.Equipment.Remove(equipment);
            _context.SaveChanges();

            return true;
        }

        public List<Equipment> SearchEquipment(string query, int? categoryId)
        {
            var equipmentQuery = _context.Equipment
                .Include(e => e.Category)
                .Include(e => e.Owner)
                .Where(e => e.IsAvailable);

            if (!string.IsNullOrEmpty(query))
            {
                equipmentQuery = equipmentQuery.Where(e =>
                    e.Name.Contains(query) || e.Description.Contains(query));
            }

            if (categoryId.HasValue)
            {
                equipmentQuery = equipmentQuery.Where(e => e.CategoryId == categoryId.Value);
            }

            return equipmentQuery.OrderByDescending(e => e.CreatedDate).ToList();
        }

    }
}