using System.Collections.Generic;
using EquipShare.Models;
using EquipShare.Models.ViewModels;

namespace EquipShare.Services
{
    public interface IEquipmentService
    {
        List<Equipment> GetAllEquipment();
        List<Equipment> GetEquipmentByOwner(int ownerId);
        Equipment GetEquipmentById(int id);
        Equipment CreateEquipment(EquipmentViewModel model, int ownerId, string imagePath);
        Equipment UpdateEquipment(EquipmentViewModel model, int ownerId, string imagePath);
        bool DeleteEquipment(int id, int ownerId);
        List<Equipment> SearchEquipment(string query, int? categoryId);
        List<Equipment> SearchAndSortEquipment(string query, int? categoryId, string sortBy);
    }
}