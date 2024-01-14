using ParkingManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagementSystem.DAL.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<bool> CreateFeedback(Feedback feedback);
        Task<List<Feedback>> GetFeedbacks();
    }
}
