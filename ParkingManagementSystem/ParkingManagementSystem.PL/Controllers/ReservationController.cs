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
        public async Task<IActionResult> CreateReservation([FromBody] ReservationRequestModel request)
        {
            try
            {
                var reservation = new Reservation
                {
                    //ReservationID = 1,
                    UserID = request.UserID,
                    LotID = request.Lot,
                    CarPlate = request.Plate,
                    StartTime = DateTime.Parse(request.Date),
                    EndTime = DateTime.Parse(request.Date).AddMinutes(request.Timestamp),
                    Status = "Pending"
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

        [HttpPost("GetReservationsByUserId")]
        public async Task<IActionResult> GetReservationsByUserId([FromBody] GetReservationRequest request)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsByUserId(request.id);
                return new JsonResult(reservations);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        [HttpPost("DeleteReservation")]
        public async Task<IActionResult> DeleteReservation([FromBody] DeleteReservationRequest request)
        {
            try
            {
                await _reservationService.DeleteReservation(request.id);
                return new JsonResult("Success!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        [HttpGet("GetAllReservations")]
        public async Task<IActionResult> GetAllReservations()
        {
            try
            {
                var reservations = await _reservationService.GetAllReservations();
                return new JsonResult(reservations);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest("Error processing the request");
            }
        }

        public class ReservationRequestModel
        {
            public int UserID { get; set; }
            public int Lot { get; set; }
            public string Date { get; set; }
            public int Timestamp { get; set; }
            public string Plate { get; set; }
        }

        public class GetReservationRequest
        {
            public int id { get; set; }
        }

        public class DeleteReservationRequest
        {
            public int id { get; set; }
        }
    }
}
