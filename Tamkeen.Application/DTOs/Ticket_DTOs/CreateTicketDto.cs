using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Tamkeen.Domain.Enums;

namespace Tamkeen.Application.DTOs.Ticket_DTOs
{
    public class CreateTicketDto
    {
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Deadline { get; set; }
        public Guid CompanyId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
