using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Validators;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ParkingManagementSystem.DAL.Repositories
{
    public interface IUserRepository
    {

        Task<bool> CreateUser(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmail(string email);
        Task<bool> AuthenticateUser(string email, string password);
        Task DeleteUser(string email);

    }
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingManagementSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False\r\n";

        public UserRepository()
        {

        }

        public async Task<bool> CreateUser(User user)
        {
            UserValidation userValidation = new UserValidation();
            if (userValidation.EmailAlreadyExists(user.Email)) return false;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

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

        public async Task<bool> AuthenticateUser(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

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



        public async Task DeleteUser(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM Users WHERE Email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
