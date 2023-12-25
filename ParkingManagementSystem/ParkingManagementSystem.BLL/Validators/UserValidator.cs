using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Validators;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ParkingManagementSystem.BLL.Validators
{
    public class UserValidator
    {
        public bool ValidateUserLogin(User user, string password)
        {
            return BCryptNet.Verify(password, user.PasswordHash);
        }

        public bool EmailAlreadyExists(string email)
        {
            UserValidation userValidation = new UserValidation();
            return userValidation.EmailAlreadyExists(email);
        }

        public bool PasswordRegex(string password)
        {
            const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
            return System.Text.RegularExpressions.Regex.IsMatch(password, passwordRegex);
        }
    }
}
