using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;

namespace ParkingManagementSystem.BLL.Interfaces
{
    public interface IFeedbackService
    {
        Task<bool> CreateFeedback(Feedback feedback);
        Task<List<Feedback>> GetFeedbacks();
    }
}
