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
    }
}
