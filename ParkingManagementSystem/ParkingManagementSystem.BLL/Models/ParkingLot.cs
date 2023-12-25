using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagementSystem.BLL.Models
{
    public class ParkingLot
    {
        public int LotID { get; set; }
        public string LotName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int CurrentAvailability { get; set; }

        // Navigation property for reservations
        public ICollection<Reservation> Reservations { get; set; }

        // Other properties as needed
    }
}
