using System;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using ParkingManagementSystem.BLL.Services;
namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False\r\n";
            ReservationRepository reservationRepository = new ReservationRepository(connectionString);

            ReservationService reservationService = new ReservationService(reservationRepository);

            Reservation reservation = new Reservation()
            {
                ReservationID = 1,
                UserID = 1,
                LotID = 1,
                CarPlate = "ABC1234",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                Status = "Active"
            };

            //Console.WriteLine(await reservationService.CreateReservation(reservation));
            Console.WriteLine(await reservationService.DeleteReservation(1));
        }
    }
}