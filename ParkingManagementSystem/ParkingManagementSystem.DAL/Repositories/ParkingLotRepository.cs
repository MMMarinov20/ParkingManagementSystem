using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Data;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Interfaces;

namespace ParkingManagementSystem.DAL.Repositories
{
    public class ParkingLotRepository : IParkingLotRepository
    {
        private readonly DatabaseConnector _databaseConnector;
        public ParkingLotRepository(DatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public async Task CreateParkingLot(ParkingLot parkingLot)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
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
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
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

        public async Task<bool> DeleteParkingLot(int lotId)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "DELETE FROM ParkingLots WHERE LotID = @LotID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LotID", lotId);

                    await command.ExecuteNonQueryAsync();

                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<List<ParkingLot>> GetAllLots()
        {
            List<ParkingLot> parkingLots = new List<ParkingLot>();

            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "SELECT * FROM ParkingLots";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ParkingLot parkingLot = new ParkingLot()
                            {
                                LotID = reader.GetInt32(0),
                                LotName = reader.GetString(1),
                                Location = reader.GetString(2),
                                Capacity = reader.GetInt32(3),
                                CurrentAvailability = reader.GetInt32(4)
                            };

                            parkingLots.Add(parkingLot);
                        }
                    }
                }
            }

            return parkingLots;
        }

        public async Task UpdateLotAvailability(int id, bool isAvailable)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "UPDATE ParkingLots SET CurrentAvailability = CurrentAvailability + @CurrentAvailability " +
                               "WHERE LotID = @LotID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LotID", id);
                    command.Parameters.AddWithValue("@CurrentAvailability", isAvailable ? 1 : -1);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
