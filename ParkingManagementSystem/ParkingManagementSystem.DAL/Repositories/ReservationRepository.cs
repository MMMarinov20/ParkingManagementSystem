using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Data;

namespace ParkingManagementSystem.DAL.Repositories
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservations();
        Task CreateReservation(Reservation reservation);
        Task EditReservation(Reservation reservation);
        Task DeleteReservation(int id);
        Task<Reservation> GetReservationById(int id);
        Task<List<Reservation>> GetReservationsByUserId(int id);
        Task UpdateStatus(int id);
    }
    public class ReservationRepository : IReservationRepository
    {
        private readonly DatabaseConnector _databaseConnector;
        private readonly IParkingLotRepository _parkingLotRepository;

        public ReservationRepository(DatabaseConnector databaseConnector, IParkingLotRepository parkingLotRepository)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
            _parkingLotRepository = parkingLotRepository ?? throw new ArgumentNullException(nameof(parkingLotRepository));
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Reservations", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Reservation reservation = new Reservation
                            {
                                ReservationID = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                LotID = reader.GetInt32(2),
                                CarPlate = reader.GetString(3),
                                StartTime = reader.GetDateTime(4),
                                EndTime = reader.GetDateTime(5),
                                Status = reader.GetString(6)
                            };

                            reservations.Add(reservation);
                        }
                    }
                }
            }

            return reservations;
        }

        public async Task CreateReservation(Reservation reservation)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "INSERT INTO Reservations (UserID, LotID, CarPlate, StartTime, EndTime, Status) " +
                               "VALUES (@UserID, @LotID, @CarPlate, @StartTime, @EndTime, @Status)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", reservation.UserID);
                    command.Parameters.AddWithValue("@LotID", reservation.LotID);
                    command.Parameters.AddWithValue("@CarPlate", reservation.CarPlate);
                    command.Parameters.AddWithValue("@StartTime", reservation.StartTime);
                    command.Parameters.AddWithValue("@EndTime", reservation.EndTime);
                    command.Parameters.AddWithValue("@Status", reservation.Status);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EditReservation(Reservation reservation)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "UPDATE Reservations SET UserID = @UserID, LotID = @LotID, CarPlate = @CarPlate, StartTime = @StartTime, EndTime = @EndTime, Status = @Status " +
                               "WHERE ReservationID = @ReservationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationID", reservation.ReservationID);
                    command.Parameters.AddWithValue("@UserID", reservation.UserID);
                    command.Parameters.AddWithValue("@LotID", reservation.LotID);
                    command.Parameters.AddWithValue("@CarPlate", reservation.CarPlate);
                    command.Parameters.AddWithValue("@StartTime", reservation.StartTime);
                    command.Parameters.AddWithValue("@EndTime", reservation.EndTime);
                    command.Parameters.AddWithValue("@Status", reservation.Status);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteReservation(int reservationId)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "DELETE FROM Reservations WHERE ReservationID = @ReservationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationID", reservationId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Reservation> GetReservationById(int reservationId)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "SELECT ReservationID, UserID, LotID, CarPlate, StartTime, EndTime, Status " +
                               "FROM Reservations " +
                               "WHERE ReservationID = @ReservationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationID", reservationId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Reservation
                            {
                                ReservationID = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                LotID = reader.GetInt32(2),
                                CarPlate = reader.GetString(3),
                                StartTime = reader.GetDateTime(4),
                                EndTime = reader.GetDateTime(5),
                                Status = reader.GetString(6)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<List<Reservation>> GetReservationsByUserId(int userId)
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "SELECT ReservationID, UserID, LotID, CarPlate, StartTime, EndTime, Status " +
                               "FROM Reservations " +
                               "WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Reservation reservation = new Reservation
                            {
                                ReservationID = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                LotID = reader.GetInt32(2),
                                CarPlate = reader.GetString(3),
                                StartTime = reader.GetDateTime(4),
                                EndTime = reader.GetDateTime(5),
                                Status = reader.GetString(6)
                            };

                            reservations.Add(reservation);
                        }
                    }
                }
            }

            return reservations;
        }
        public async Task UpdateStatus(int id)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "UPDATE Reservations SET Status = 'Cancelled' WHERE ReservationID = @ReservationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationID", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
