using System.Collections.Generic;
using System.Linq;
using MyApp.Cross.Models;

namespace MyApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyAppDbContext _db;

        public UserRepository(MyAppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public User? GetUserByUsername(string username)
        {
            return _db.Users.FirstOrDefault(u => u.Username == username);
        }

        public User? CreateUser(string username, string passwordHash)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash
            };
            _db.Users.Add(user);
            int rows = _db.SaveChanges();
            return (rows > 0) ? user : null;
        }

        public bool UpdateUser(int id, string username, string passwordHash)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            user.Username = username;
            user.PasswordHash = passwordHash;
            int rows = _db.SaveChanges();
            return (rows > 0);
        }

        public bool DeleteUser(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            _db.Users.Remove(user);
            int rows = _db.SaveChanges();
            return (rows > 0);
        }
    }
}
