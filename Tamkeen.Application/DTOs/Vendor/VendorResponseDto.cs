using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamkeen.Application.DTOs.Vendor
{
    public class VendorResponseDto
    {
        public string fullName { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string specialty { get; set; } = null!;
        public int yearsExperience { get; set; }
        public string? bio { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
