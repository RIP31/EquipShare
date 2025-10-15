using System.Linq;
using EquipShare.Data;
using EquipShare.Models;
using EquipShare.Models.ViewModels;
using EquipShare.Utilities;

namespace EquipShare.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email);

            // Check if user exists and password is correct
            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
                return null;

            // Authentication successful
            return user;
        }

        public User Register(RegisterViewModel model)
        {
            // Check if email already exists
            if (_context.Users.Any(x => x.Email == model.Email))
                return null;

            // Map view model to user entity
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = PasswordHasher.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.SingleOrDefault(x => x.Id == userId);
        }
    }
}