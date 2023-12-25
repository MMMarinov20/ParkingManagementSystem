using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Models;

namespace ParkingManagementSystem.DAL.Repositories
{
    public class ParkingLotRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False\r\n";

        public ParkingLotRepository(string connectionString)
        {
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task CreateParkingLot(ParkingLot parkingLot)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO ParkingLots (LotID, LotName, Location, Capacity, CurrentAvailability) " +
                               "VALUES (@LotID, @LotName, @Location, @Capacity, @CurrentAvailability)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LotID", parkingLot.LotID);
                    command.Parameters.AddWithValue("@LotName", parkingLot.LotName);
                    command.Parameters.AddWithValue("@Location", parkingLot.Location);
                    command.Parameters.AddWithValue("@Capacity", parkingLot.Capacity);
                    command.Parameters.AddWithValue("@CurrentAvailability", parkingLot.CurrentAvailability);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EditParkingLot(ParkingLot parkingLot)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE ParkingLots SET LotName = @LotName, Location = @Location, Capacity = @Capacity, CurrentAvailability = @CurrentAvailability " +
                               "WHERE LotID = @LotID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LotID", parkingLot.LotID);
                    command.Parameters.AddWithValue("@LotName", parkingLot.LotName);
                    command.Parameters.AddWithValue("@Location", parkingLot.Location);
                    command.Parameters.AddWithValue("@Capacity", parkingLot.Capacity);
                    command.Parameters.AddWithValue("@CurrentAvailability", parkingLot.CurrentAvailability);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteParkingLot(int lotId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM ParkingLots WHERE LotID = @LotID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LotID", lotId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
