using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
//using ParkingManagementSystem.BLL.Validators;
using ParkingManagementSystem.BLL.Interfaces;
//using BCryptNet = BCrypt.Net.BCrypt;

namespace ParkingManagementSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        public async Task CreateUser(User user)
        {
            //if (_userValidator.EmailAlreadyExists(user.Email) || !_userValidator.PasswordRegex(user.PasswordHash))
            //{
            //    return false;
            //}

            //user.PasswordHash = BCryptNet.HashPassword(user.PasswordHash);
            await _userRepository.CreateUser(user);

            //return true;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        //public async Task<bool> AuthenticateUser(string email, string password)
        //{
        //    User user = await _userRepository.GetUserByEmail(email);

        //    if (user == null) return false;

        //    return BCryptNet.Verify(password, user.PasswordHash);
        //}

        public Task<User> GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public async Task<bool> AuthenticateUser(string email, string password)
        {
            return await _userRepository.AuthenticateUser(email, password);
        }

        public async Task DeleteUser(string email)
        {
            await _userRepository.DeleteUser(email);
        }
    }
}
