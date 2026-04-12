using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamkeen.Application.DTOs.Ticket_DTOs
{
    public class ImageResponseDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }
}
