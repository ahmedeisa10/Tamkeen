using Microsoft.AspNetCore.Identity;
namespace Tamkeen.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? ImageUrl { get; set; } // جديد

        public string? EmailConfirmationCode { get; set; }
        public DateTime? CodeExpiry { get; set; }

        // الـ Feedbacks اللي كتبها كـ Tenant
        public ICollection<Feedback> TenantFeedbacks { get; set; }

        // الـ Feedbacks اللي اتقيّم فيها كـ Vendor
        public ICollection<Feedback> VendorFeedbacks { get; set; }
        public ICollection<TicketApplication> Applications { get; set; }
    }
}
