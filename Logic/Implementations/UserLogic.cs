using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MyApp.Cross.Models;
using MyApp.Cross.Utils;
using MyApp.Logic.Interfaces;
using MyApp.Repository;

namespace MyApp.Logic.Implementations
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public UserLogic(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _repo.GetAllUsers();
        }

        public User? CreateUser(string username, string password)
        {
            var hash = BcryptUtils.HashPassword(password);
            return _repo.CreateUser(username, hash);
        }

        public bool UpdateUser(int id, string username, string password)
        {
            var hash = BcryptUtils.HashPassword(password);
            return _repo.UpdateUser(id, username, hash);
        }

        public bool DeleteUser(int id)
        {
            return _repo.DeleteUser(id);
        }

        public string? Login(string username, string password)
        {
            var user = _repo.GetUserByUsername(username);
            if (user == null) return null;
            bool valid = BcryptUtils.VerifyPassword(password, user.PasswordHash);
            if (!valid) return null;
            return JwtUtils.GenerateToken(user.Username, _config);
        }
    }
}
