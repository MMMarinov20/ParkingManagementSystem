﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using ParkingManagementSystem.BLL.Interfaces;

namespace ParkingManagementSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CreateUser(User user)
        {
            return await _userRepository.CreateUser(user);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public Task<User> GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public async Task<bool> AuthenticateUser(string email, string password)
        {
            return await _userRepository.AuthenticateUser(email, password);
        }

        public async Task<bool> DeleteUser(int id, string password)
        {
            return await _userRepository.DeleteUser(id, password);
        }

        public void UpdateUser(User user, string oldPassword) {
            _userRepository.UpdateUser(user, oldPassword);
        }
    }
}
