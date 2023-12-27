using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ParkingManagementSystem.BLL.Interfaces;
using ParkingManagementSystem.DAL.Models;

namespace ParkingManagementSystemPL.Pages
{
    public class LotsModel : PageModel
    {
        private readonly IReservationService _reservationService;
        public void OnGet()
        {
        }

        public LotsModel(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> onButtonClick()
        {
            //var reservation = new Reservation
            //{
            //    ReservationID = 1,
            //    UserID = 1,
            //    LotID = 1,
            //    CarPlate = "123456",
            //    StartTime = DateTime.Now,
            //    EndTime = DateTime.Now.AddDays(1),
            //    Status = "Active"
            //};

            //await _reservationService.CreateReservation(reservation);

            return new JsonResult("Success");
        }
    }
}
