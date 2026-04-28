namespace Tamkeen.Domain.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
        public ICollection<Ticket> Tickets { get; set; }

        //public DateTime CreatedAt { get; set; }
        //public Guid CreatedBy { get; set; }
        //public DateTime ModifiedAt { get; set; }
        //public Guid? ModifiedBy { get; set; }
        //public DateTime DeletedAt { get; set; }
        //public Guid? ModifiedBy { get; set; }
    }
}
