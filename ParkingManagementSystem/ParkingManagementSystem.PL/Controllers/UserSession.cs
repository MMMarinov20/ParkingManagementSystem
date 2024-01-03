using ParkingManagementSystem.DAL.Models;

namespace ParkingManagementSystemPL.Controllers
{
    public class UserSession
    {
        private static UserSession _instance;

        public User currentUser { get; set; }

        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSession();
                }
                return _instance;
            }
        }
    }
}
