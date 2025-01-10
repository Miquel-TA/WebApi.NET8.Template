using System.Collections.Generic;
using MyApp.Cross.Models;

namespace MyApp.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User? CreateUser(string username, string passwordHash);
        bool UpdateUser(int id, string username, string passwordHash);
        bool DeleteUser(int id);
        User? GetUserByUsername(string username);
    }
}
