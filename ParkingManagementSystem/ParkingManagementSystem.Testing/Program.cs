using System;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False\r\n";
            UserRepository userRepository = new UserRepository(connectionString);

            int userIdToRetrieve = 1; // Replace with a valid user ID from your database
            User retrievedUser = await userRepository.GetUserByIdAsync(userIdToRetrieve);

            // Print or use the retrieved user
            if (retrievedUser != null)
            {
                Console.WriteLine($"Retrieved User: {retrievedUser.FirstName} {retrievedUser.LastName}");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
    }
}