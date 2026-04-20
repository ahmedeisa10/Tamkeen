using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamkeen.Application.DTOs.Vendor
{
    public class VendorProfileDto
    {
        public string VendorId { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string specialty { get; set; } = null!;
        public int yearsExperience { get; set; }
        public string? bio { get; set; } = null!;
        public List<VendorTicketHistoryDto> TicketHistory { get; set; }
    }
}
