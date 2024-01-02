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
        private readonly IUserService _authenticationService;

        public UserController(IUserService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            //get the type of user.email
            string email = user.Email;
            string password = user.PasswordHash;
            try
            {
                if (await _authenticationService.AuthenticateUser(email, password))
                {
                    return new JsonResult("Success");
                }
                else
                {
                    return new JsonResult("Wrong password");
                }

                //return new JsonResult("Success");

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
    }
}
