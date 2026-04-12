using Microsoft.AspNetCore.Identity;
namespace Tamkeen.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime? CodeExpiry { get; set; }
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
