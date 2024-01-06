using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Validators;
using ParkingManagementSystem.DAL.Data;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ParkingManagementSystem.DAL.Repositories
{
    public interface IUserRepository
    {

        Task<bool> CreateUser(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmail(string email);
        Task<bool> AuthenticateUser(string email, string password);
        Task<bool> DeleteUser(int id, string password);

    }
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnector _databaseConnector;

        public UserRepository(DatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public async Task<bool> CreateUser(User user)
        {
            UserValidation userValidation = new UserValidation();
            if (userValidation.EmailAlreadyExists(user.Email)) return false;
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {

                string query = "INSERT INTO Users (FirstName, LastName, Email, Password, Phone) " +
                               "VALUES (@FirstName, @LastName, @Email, @Password, @Phone)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", BCryptNet.HashPassword(user.PasswordHash));
                    command.Parameters.AddWithValue("@Phone", user.Phone);

                    await command.ExecuteNonQueryAsync();

                    return true;
                }
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {

                string query = "SELECT * FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                //UserID = (int)reader["UserID"],
                                //FirstName = reader["FirstName"].ToString(),
                                //LastName = reader["LastName"].ToString(),
                                //Email = reader["Email"].ToString(),
                                //PasswordHash = reader["Password"].ToString(),
                                //Phone = reader["Phone"].ToString()
                                UserID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                PasswordHash = reader.GetString(4),
                                Phone = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {

                string query = "SELECT * FROM Users WHERE Email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                PasswordHash = reader.GetString(4),
                                Phone = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> AuthenticateUser(string email, string password)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {

                string query = "SELECT Password FROM Users WHERE Email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Console.WriteLine("asd");
                            string storedPassword = reader["Password"].ToString();
                            Console.WriteLine($"Stored Password: {storedPassword}");
                            return BCryptNet.Verify(password, storedPassword);
                        }
                        else
                        {
                            Console.WriteLine("Email not found");
                            return false;
                        }
                    }
                }
            }
        }



        public async Task<bool> DeleteUser(int id, string password)
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "SELECT Password FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string storedPassword = reader["Password"].ToString();
                            if (BCryptNet.Verify(password, storedPassword))
                            {
                                // Close the SqlDataReader before executing additional commands
                                reader.Close();

                                // Now you can execute additional commands
                                query = "DELETE FROM Reservations WHERE UserID = @UserID";
                                using (SqlCommand command2 = new SqlCommand(query, connection))
                                {
                                    command2.Parameters.AddWithValue("@UserID", id);
                                    await command2.ExecuteNonQueryAsync();
                                }

                                query = "DELETE FROM Users WHERE UserID = @UserID";
                                using (SqlCommand command3 = new SqlCommand(query, connection))
                                {
                                    command3.Parameters.AddWithValue("@UserID", id);
                                    await command3.ExecuteNonQueryAsync();
                                }

                                return true;
                            }
                            return false;
                        }
                    }
                }
                return false;
            }
        }
    }
}
