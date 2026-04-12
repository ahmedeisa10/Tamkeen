namespace Tamkeen.Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }

        public Guid RequestId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
