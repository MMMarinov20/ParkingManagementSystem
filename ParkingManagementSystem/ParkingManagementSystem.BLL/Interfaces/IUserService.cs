using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
//using ParkingManagementSystem.BLL.Validators;
using ParkingManagementSystem.BLL.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ParkingManagementSystem.BLL.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmail(string email);
        Task<bool> AuthenticateUser(string email = "example", string password = "example");
        Task DeleteUser(string email);

    }
}
