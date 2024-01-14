using ParkingManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagementSystem.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmail(string email);
        Task<bool> AuthenticateUser(string email, string password);
        Task<bool> DeleteUser(int id, string password);
        Task<bool> DeleteUserById(int id);
        Task<bool> UpdateUser(User user, string oldPassword);
        Task<List<User>> GetAllUsers();
        Task PromoteUser(int id);
    }
}
