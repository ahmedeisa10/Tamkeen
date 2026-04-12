using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamkeen.Domain.Entities;
namespace Tamkeen.Infrastructure.Data.EntityTypeConfiguration
{
    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(x => x.Ticket)
                .WithMany(t => t.Images)
                .HasForeignKey(x => x.TicketId);
        }
    }
}
