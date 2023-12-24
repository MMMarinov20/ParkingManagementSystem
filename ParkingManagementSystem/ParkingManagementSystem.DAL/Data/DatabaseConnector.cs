using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ParkingManagementSystem.DAL.Data
{
    public class DatabaseConnector
    {
        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False\r\n";

        public SqlConnection GetOpenConnection()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Database connection opened successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening database connection: {ex.Message}");
                connection.Close();
                connection = null;
            }

            return connection;
        }

        public void CloseConnection(SqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Database connection closed.");
            }
        }
    }
}
