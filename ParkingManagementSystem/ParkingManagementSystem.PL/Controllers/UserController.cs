﻿using Microsoft.AspNetCore.Mvc;
using ParkingManagementSystem.BLL.Interfaces;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystemPL.Pages;
using System.Text.Json;


namespace ParkingManagementSystemPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _authenticationService;

        public UserController(IUserService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            string email = user.Email;
            string password = user.PasswordHash;
            try
            {
                if (await _authenticationService.AuthenticateUser(email, password))
                {
                    User currentUser = await _authenticationService.GetUserByEmail(email);

                    HttpContext.Session.SetString("CurrentUser", JsonSerializer.Serialize(currentUser));
                    return new JsonResult("Success!");
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            User newUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Phone = user.Phone
            };
            if (await _authenticationService.CreateUser(newUser))
            {
                return new JsonResult("Success!");
            }
            else
            {
                return new JsonResult("User already exists");
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            //UserSession.Instance.currentUser = null;
            HttpContext.Session.Clear();

            return new JsonResult("Success!");
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest user)
        {
            try
            {
                var currentUser = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("CurrentUser"));
                if (await _authenticationService.DeleteUser(currentUser.UserID, user.password))
                {
                    HttpContext.Session.Clear();
                    return new JsonResult("Success!");
                }
                return new JsonResult("Wrong credentials!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        [HttpPost("DeleteUserById")]
        public async Task<IActionResult> DeleteUserById([FromBody] DeleteUserByIdRequest user)
        {
            try
            {
                if (await _authenticationService.DeleteUserById(user.id))
                {
                    return new JsonResult("Success!");
                }
                return new JsonResult("Wrong credentials!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest user)
        {
            var currentUser = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("CurrentUser"));
            Console.WriteLine(currentUser.UserID);
            User updatedUser = new User()
            {
                UserID = currentUser.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.NewPassword,
                Phone = user.Phone
            };
            try
            {
                if (await _authenticationService.UpdateUser(updatedUser, user.OldPassword))
                {
                    HttpContext.Session.SetString("CurrentUser", JsonSerializer.Serialize(updatedUser));
                    return new JsonResult("Success!");
                }
                return new JsonResult("Email already exists or wrong password!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return new JsonResult(await _authenticationService.GetAllUsers());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        [HttpPost("PromoteUser")]
        public async Task<IActionResult> PromoteUser([FromBody] DeleteUserByIdRequest user)
        {
            try
            {
                await _authenticationService.PromoteUser(user.id);
                return new JsonResult("Success!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        [HttpPost("GetUserById")]
        public async Task<IActionResult> GetUserById([FromBody] DeleteUserByIdRequest user)
        {
            User userFromDb =  await _authenticationService.GetUserByIdAsync(user.id);
            return new JsonResult(userFromDb);
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string PasswordHash { get; set; }
        }

        public class RegisterRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public string Phone { get; set; }
        }

        public class DeleteUserRequest
        {
            public string password { get; set; }
        }

        public class DeleteUserByIdRequest
        {
            public int id { get; set; }
        }

        public class UpdateUserRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string Phone { get; set; }
        }
    }
}
