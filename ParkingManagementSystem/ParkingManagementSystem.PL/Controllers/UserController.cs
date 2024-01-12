using Microsoft.AspNetCore.Mvc;
using ParkingManagementSystem.BLL.Interfaces;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystemPL.Pages;

namespace ParkingManagementSystemPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UserSession session = UserSession.Instance;
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
                    session.currentUser = currentUser;
                    return new JsonResult("Success!");
                }
                else
                {
                    return new JsonResult("Invalid credentials!");
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
            UserSession.Instance.currentUser = null;

            return new JsonResult("Success!");
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest user)
        {
            try
            {
                if (await _authenticationService.DeleteUser(UserSession.Instance.currentUser.UserID, user.password))
                {
                    UserSession.Instance.currentUser = null;
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
            User updatedUser = new User()
            {
                UserID = UserSession.Instance.currentUser.UserID,
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
                    UserSession.Instance.currentUser = updatedUser;
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
