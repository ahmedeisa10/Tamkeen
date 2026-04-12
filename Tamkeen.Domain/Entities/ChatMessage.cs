namespace Tamkeen.Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }

        public string SenderId { get; set; }
        public AppUser Sender { get; set; }

        public Guid RequestId { get; set; }
        public Ticket Ticket { get; set; }

        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
