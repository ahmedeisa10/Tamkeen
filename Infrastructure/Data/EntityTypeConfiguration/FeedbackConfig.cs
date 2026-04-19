using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamkeen.Domain.Entities;

namespace Tamkeen.Infrastructure.Data.EntityTypeConfiguration
{
    public class FeedbackConfig : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.Property(x => x.Comment)
                .HasMaxLength(1000);


            builder.HasOne(f => f.Tenant)
                .WithMany(u => u.TenantFeedbacks)
                .HasForeignKey(f => f.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Vendor)
                .WithMany(u => u.VendorFeedbacks)
                .HasForeignKey(f => f.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}