using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamkeen.Domain.Entities;

namespace Tamkeen.Infrastructure.Data.EntityTypeConfiguration
{
    public class TicketApplicationConfig : IEntityTypeConfiguration<TicketApplication>
    {
        public void Configure(EntityTypeBuilder<TicketApplication> builder)
        {
            // Vendor apply only once on the same ticket
            builder.HasIndex(a => new { a.TicketId, a.VendorId })
                   .IsUnique();

            //  Relation with Ticket
            builder.HasOne(a => a.Ticket)
                   .WithMany(t => t.Applications)
                   .HasForeignKey(a => a.TicketId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relation with Vendor
            builder.HasOne(a => a.Vendor)
                   .WithMany(u => u.Applications)
                   .HasForeignKey(a => a.VendorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}