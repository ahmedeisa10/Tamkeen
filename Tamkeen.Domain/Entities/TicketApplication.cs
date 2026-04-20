namespace Tamkeen.Domain.Entities
{
    public class TicketApplication
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string VendorId { get; set; }
        public AppUser Vendor { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}