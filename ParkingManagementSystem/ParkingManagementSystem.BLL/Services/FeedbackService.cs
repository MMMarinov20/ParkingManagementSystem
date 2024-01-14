using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.BLL.Interfaces;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using ParkingManagementSystem.DAL.Interfaces;

namespace ParkingManagementSystem.BLL.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
        }

        public async Task<bool> CreateFeedback(Feedback feedback)
        {
            return await _feedbackRepository.CreateFeedback(feedback);
        }

        public async Task<List<Feedback>> GetFeedbacks()
        {
            return await _feedbackRepository.GetFeedbacks();
        }
    }
}
