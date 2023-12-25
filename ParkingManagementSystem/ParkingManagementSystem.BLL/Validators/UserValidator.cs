using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;

namespace ParkingManagementSystem.BLL.Validators
{
    public class UserValidator
    {
        public bool ValidateUserLogin(User user, string password)
        {
            return password == user.PasswordHash;
        }
    }
}
