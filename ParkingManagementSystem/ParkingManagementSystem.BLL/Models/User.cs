using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagementSystem.BLL.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string CarPlate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }

        // Navigation property for reservations
        public ICollection<Reservation> Reservations { get; set; }

    }
}
