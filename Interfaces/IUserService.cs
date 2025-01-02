using dotNET.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace dotNET.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(string login, string password, string firstName, string lastName, string email);
        Task<User> AuthenticateUserAsync(string login, string password);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUserAsync(string login, string password, string firstName, string lastName,
            string email)
        {
            if (await _context.Users.AnyAsync(u => u.Login == login))
                throw new Exception("User already exists.");

            var passwordHash = HashPassword(password);

            var user = new User
            {
                Login = login,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> AuthenticateUserAsync(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null)
                throw new Exception("User not found.");

            if (!VerifyPassword(password, user.PasswordHash))
                throw new Exception("Invalid password.");

            return user;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}