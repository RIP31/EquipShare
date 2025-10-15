using EquipShare.Models;
using EquipShare.Models.ViewModels;

namespace EquipShare.Services
{
    public interface IAuthService
    {
        User Authenticate(string email, string password);
        User Register(RegisterViewModel model);
        bool EmailExists(string email);
        User GetUserById(int userId);
    }
}