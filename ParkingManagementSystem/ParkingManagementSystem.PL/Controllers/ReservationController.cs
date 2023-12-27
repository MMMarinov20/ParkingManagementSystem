using Microsoft.AspNetCore.Mvc;
using ParkingManagementSystem.BLL.Interfaces;
using ParkingManagementSystem.DAL.Models;
using System;
using System.Threading.Tasks;

namespace ParkingManagementSystemPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("CreateReservation")]
        public async Task<IActionResult> CreateReservation()
        {
            try
            {
                var reservation = new Reservation
                {
                    ReservationID = 1,
                    UserID = 1,
                    LotID = 1,
                    CarPlate = "123456",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(1),
                    Status = "Active"
                };

                await _reservationService.CreateReservation(reservation);

                return new JsonResult("Success");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }
    }
}
