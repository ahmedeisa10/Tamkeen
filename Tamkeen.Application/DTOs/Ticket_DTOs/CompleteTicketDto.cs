// Tamkeen.Application.DTOs.Ticket_DTOs/CompleteTicketDto.cs
using Microsoft.AspNetCore.Http;

namespace Tamkeen.Application.DTOs.Ticket_DTOs
{
    public class CompleteTicketDto
    {
        public List<IFormFile> Images { get; set; }
    }
}