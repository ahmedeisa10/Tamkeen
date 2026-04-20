using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tamkeen.Application.DTOs;
using Tamkeen.Application.DTOs.Feedback;
using Tamkeen.Application.Interfaces;
using Tamkeen.Application.Interfaces.Feedback;

namespace Tamkeen.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FeedbacksController(IFeedbackService _feedbackService) : ControllerBase
    {
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // POST /api/Feedbacks
        [HttpPost]
        [Authorize(Roles = "Tenant")]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackDto dto)
        {
            var result = await _feedbackService.CreateAsync(dto, GetUserId());
            return Ok(result);
        }

        // GET /api/Feedbacks/ticket/{ticketId}
        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetByTicket(Guid ticketId)
        {
            var result = await _feedbackService.GetByTicketAsync(ticketId);
            return Ok(result);
        }
    }
}