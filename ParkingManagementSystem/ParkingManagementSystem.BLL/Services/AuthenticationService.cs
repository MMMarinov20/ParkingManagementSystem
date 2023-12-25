﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using ParkingManagementSystem.BLL.Validators;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ParkingManagementSystem.BLL.Services
{
    public class AuthenticationService
    {
        private readonly UserRepository _userRepository;
        UserValidator userValidator = new UserValidator();

        public AuthenticationService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AuthenticateUser(string email, string password)
        {
            User user = await _userRepository.GetUserByEmail(email);

            if (user == null) return false;

            return userValidator.ValidateUserLogin(user, password);
        }

        public async Task<bool> RegisterUser(User user)
        {
            if (userValidator.EmailAlreadyExists(user.Email) || !userValidator.PasswordRegex(user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = BCryptNet.HashPassword(user.PasswordHash);
            await _userRepository.CreateUser(user);
            
            return true;
        }
    }
}
