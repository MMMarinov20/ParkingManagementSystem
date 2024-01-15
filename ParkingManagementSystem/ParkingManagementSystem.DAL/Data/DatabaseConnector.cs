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
        //private readonly string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False\r\n";
        private readonly string ConnectionString = "Server=tcp:parkingmanagementsystemmax.database.windows.net,1433;Initial Catalog=ParkingManagementSystem;Persist Security Info=False;User ID=MMMarinov20;Password=_ParkingManagementSystem123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
