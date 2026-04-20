using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamkeen.Application.DTOs.Feedback
{
    public class CreateFeedbackDto
    {
        public string Comment { get; set; }
        public Guid TicketId { get; set; }
        public string VendorId { get; set; }
    }
}
