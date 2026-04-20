using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.Application.DTOs.Feedback;

namespace Tamkeen.Application.DTOs.Vendor
{
    public class VendorTicketHistoryDto
    {
        public Guid TicketId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<FeedbackDto> Feedbacks { get; set; }
    }
}
