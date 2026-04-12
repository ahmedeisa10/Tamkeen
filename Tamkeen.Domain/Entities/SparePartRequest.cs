namespace Tamkeen.Domain.Entities
{
    public class SparePartRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public bool IsApproved { get; set; }

        public Guid RequestId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
