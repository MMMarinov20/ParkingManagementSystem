using ParkingManagementSystem.DAL.Data;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using Microsoft.Data.SqlClient;
namespace ParkingManagementSystem.Tests
{
    [TestFixture]
    public class ReservationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(1)]
        public async Task UserCanCreateReservationSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            ParkingLotRepository parkingLotRepository = new ParkingLotRepository(databaseConnector);
            ReservationRepository reservationRepository = new ReservationRepository(databaseConnector, parkingLotRepository);
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);
            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            Reservation reservation = new Reservation();
            reservation.UserID = userFromDatabase.UserID;
            reservation.LotID = 1;
            reservation.CarPlate = "Test123";
            reservation.StartTime = DateTime.Now;
            reservation.EndTime = DateTime.Now.AddMinutes(1);
            reservation.Status = "Active";

            await reservationRepository.CreateReservation(reservation);

            List<Reservation> reservations = await reservationRepository.GetReservationsByUserId(userFromDatabase.UserID);
            Assert.IsNotNull(reservations);

            await reservationRepository.DeleteReservation(reservations[0].ReservationID);
            await userRepository.DeleteUser(userFromDatabase.UserID, user.PasswordHash);
        }

        [Test, Order(2)]
        public async Task UserCanDeleteReservationSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            ParkingLotRepository parkingLotRepository = new ParkingLotRepository(databaseConnector);
            ReservationRepository reservationRepository = new ReservationRepository(databaseConnector, parkingLotRepository);
            UserRepository userRepository = new UserRepository(databaseConnector);


            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            await userRepository.CreateUser(user);

            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            Reservation reservation = new Reservation();

            reservation.UserID = userFromDatabase.UserID;
            reservation.LotID = 1;
            reservation.CarPlate = "Test123";
            reservation.StartTime = DateTime.Now;
            reservation.EndTime = DateTime.Now.AddMinutes(1);
            reservation.Status = "Active";

            await reservationRepository.CreateReservation(reservation);
            List<Reservation> reservationFromDatabase = await reservationRepository.GetReservationsByUserId(userFromDatabase.UserID);

            await reservationRepository.DeleteReservation(reservationFromDatabase[0].ReservationID);

            List<Reservation> reservationFromDatabaseAfterDelete = await reservationRepository.GetReservationsByUserId(userFromDatabase.UserID);

            Assert.IsEmpty(reservationFromDatabaseAfterDelete);

            await userRepository.DeleteUser(userFromDatabase.UserID, user.PasswordHash);
        }
    }
}
