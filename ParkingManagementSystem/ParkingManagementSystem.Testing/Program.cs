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
            //string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False\r\n";
            //UserRepository userRepository = new UserRepository();
            //UserService authenticationService = new UserService(userRepository);
            ////fname, lastname, email, phone
            //var user = new User
            //{
            //    FirstName = "John",
            //    LastName = "Doe",
            //    Email = "exmaplesjh@",
            //    PasswordHash = "Maks123.",
            //    Phone = "1234567890"
            //};

            //Console.WriteLine(await authenticationService.RegisterUser(user));
        }
    }
}