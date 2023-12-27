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
            ParkingLotRepository parkingLotRepository = new ParkingLotRepository(connectionString);

            ParkingLotService parkingLotService = new ParkingLotService(parkingLotRepository);

            ParkingLot parkingLot = new ParkingLot()
            {
                LotID = 2,
                LotName = "Lot 1",
                Location = "1234 Main St.",
                Capacity = 100,
                CurrentAvailability = 100
            };

            Console.WriteLine("Creating parking lot...");
            Console.WriteLine(await parkingLotService.DeleteParkingLot(2));
        }
    }
}