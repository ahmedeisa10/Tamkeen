using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Tamkeen.Application.DTOs.Ticket_DTOs;
using Tamkeen.Application.Interfaces.Ticket_Interface;

namespace Tamkeen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        private string GetUserRole() =>
            User.FindFirstValue(ClaimTypes.Role)!;

        // GET /api/tickets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync(GetUserId(), GetUserRole());
            return Ok(tickets);
        }

        // GET /api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _ticketService.GetByIdAsync(id, GetUserId(), GetUserRole());
            return Ok(ticket);
        }

        // POST /api/tickets
        [HttpPost]
        //[Authorize(Roles = "Tenant")]
        public async Task<IActionResult> Create([FromForm] CreateTicketDto dto)
        {
            var ticket = await _ticketService.CreateAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
        }

        [HttpPatch("{id}/assign")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> AssignVendor(Guid id, [FromBody] AssignTicketDto dto)
        {
            await _ticketService.AssignVendorAsync(id, dto);
            return NoContent();
        }

        // PATCH /api/tickets/{id}/accept
        [HttpPatch("{id}/accept")]
        //[Authorize(Roles = "Vendor")]
        public async Task<IActionResult> Accept(Guid id)
        {
            await _ticketService.AcceptAsync(id, GetUserId());
            return NoContent();
        }

        // PATCH /api/tickets/{id}/reject
        [HttpPatch("{id}/reject")]
        //[Authorize(Roles = "Vendor")]
        public async Task<IActionResult> Reject(Guid id)
        {
            await _ticketService.RejectAsync(id, GetUserId());
            return NoContent();
        }

        // PATCH /api/tickets/{id}/complete
        [HttpPatch("{id}/complete")]
        //[Authorize(Roles = "Vendor")]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _ticketService.CompleteAsync(id, GetUserId());
            return NoContent();
        }

        // PATCH /api/tickets/{id}/close
        [HttpPatch("{id}/close")]
        //[Authorize(Roles = "Tenant")]
        public async Task<IActionResult> Close(Guid id)
        {
            await _ticketService.CloseAsync(id, GetUserId());
            return NoContent();
        }


        // POST /api/tickets/{id}/images
        [HttpPost("{id}/images")]
        //[Authorize(Roles = "Vendor")] //Vendor upload images after job complete
        public async Task<IActionResult> UploadImages(Guid id, [FromForm] UploadTicketImagesDto dto)
        {
           

            var images = await _ticketService.UploadImagesAsync(id, dto, GetUserId());
            return Ok(images);
        }

    }
}
