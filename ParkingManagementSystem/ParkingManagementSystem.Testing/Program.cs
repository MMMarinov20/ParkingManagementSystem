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
            UserRepository userRepository = new UserRepository(connectionString);
            User user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "example@1",
                PasswordHash = "123",
                Phone = "123"
            };

        }
    }
}