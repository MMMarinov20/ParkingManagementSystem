using System;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Data;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Replace this with your actual connection string
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False\r\n";

            // Create an instance of the DatabaseConnector
            DatabaseConnector dbConnector = new DatabaseConnector();

            // Create a SqlConnection using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the database connection
                connection.Open();

                // Perform database operations here

                // Example: Query database
                using (SqlCommand command = new SqlCommand("SELECT * FROM Users", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Process data
                            Console.WriteLine($"UserID: {reader["UserID"]}, FirstName: {reader["FirstName"]}, LastName: {reader["LastName"]}");
                        }
                    }
                }

                // Close the database connection
                connection.Close();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}