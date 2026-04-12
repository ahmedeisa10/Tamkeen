using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamkeen.Application.DTOs.Ticket_DTOs
{
    public class TicketResponseDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string? VendorId { get; set; }
        public string? VendorName { get; set; }
        public Guid CompanyId { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
