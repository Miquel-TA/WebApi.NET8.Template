using System.Collections.Generic;
using MyApp.Cross.Models;

namespace MyApp.Logic.Interfaces
{
    public interface IUserLogic
    {
        IEnumerable<User> GetAllUsers();
        User? CreateUser(string username, string password);
        bool UpdateUser(int id, string username, string password);
        bool DeleteUser(int id);
        string? Login(string username, string password);
    }
}
