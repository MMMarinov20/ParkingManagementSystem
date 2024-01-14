using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Data;
using ParkingManagementSystem.DAL.Models;
using Microsoft.Data.SqlClient;

namespace ParkingManagementSystem.DAL.Repositories
{
    public interface IFeedbackRepository
    {
        Task<bool> CreateFeedback(Feedback feedback);
        Task<List<Feedback>> GetFeedbacks();
    }

    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly DatabaseConnector _databaseConnector;

        public FeedbackRepository(DatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public async Task<bool> CreateFeedback(Feedback feedback)
        {
            using(SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "INSERT INTO Feedback (UserID, Rating, Comment) VALUES (@UserID, @Rating, @Comment)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", feedback.UserID);
                    command.Parameters.AddWithValue("@Rating", feedback.Rating);
                    command.Parameters.AddWithValue("@Comment", feedback.Comment);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected == 1;
                }
            }
        }

        public async Task<List<Feedback>> GetFeedbacks()
        {
            using (SqlConnection connection = _databaseConnector.GetOpenConnection())
            {
                string query = "SELECT * FROM Feedback";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        List<Feedback> feedbacks = new List<Feedback>();

                        while (await reader.ReadAsync())
                        {
                            Feedback feedback = new Feedback
                            {
                                Id = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                Rating = reader.GetInt32(2),
                                Comment = reader.GetString(3)
                            };

                            feedbacks.Add(feedback);
                        }

                        return feedbacks;
                    }
                }
            }
        }
    }
}
