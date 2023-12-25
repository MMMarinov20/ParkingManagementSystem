﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Models;

namespace ParkingManagementSystem.DAL.Repositories
{
    public class ReservationRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False\r\n";

        public ReservationRepository(string connectionString)
        {
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task CreateReservation(Reservation reservation)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO Reservations (ReservationID, UserID, LotID, CarPlate, StartTime, EndTime, Status) " +
                               "VALUES (@ReservationID, @UserID, @LotID, @CarPlate, @StartTime, @EndTime, @Status)";

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

        public async Task EditReservation(Reservation reservation)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

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
    }
}