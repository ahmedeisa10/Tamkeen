using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Tamkeen.Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }

        public string? Comment { get; set; }

        [ForeignKey("Ticket")]
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }


        // Tenant
        [ForeignKey("Tenant")]
        public string TenantId { get; set; }
        public AppUser Tenant { get; set; }

        // Vendor
        [ForeignKey("Vendor")]
        public string VendorId { get; set; }
        public AppUser Vendor { get; set; }
    }
}
