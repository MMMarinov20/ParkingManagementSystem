using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ParkingManagementSystem.BLL.Interfaces;
using ParkingManagementSystem.DAL.Models;
using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace ParkingManagementSystemPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("CreateFeedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequest feedback)
        {
            var currentUser = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("CurrentUser"));
            Feedback feedbackToCreate = new Feedback
            {
                UserID = currentUser.UserID,
                Comment = feedback.Comment,
                Rating = feedback.Rating,
            };
            if (await _feedbackService.CreateFeedback(feedbackToCreate))
            {
                return new JsonResult("Success!");
            }

            return BadRequest("Faled to create feedback");
        }

        [HttpGet("GetAllFeedbacks")]
        public async Task<List<Feedback>> GetAllFeedbacks()
        {
            return await _feedbackService.GetFeedbacks();
        }

        public class FeedbackRequest
        {
            public string Comment { get; set; }
            public int Rating { get; set; }
        }
    }
}