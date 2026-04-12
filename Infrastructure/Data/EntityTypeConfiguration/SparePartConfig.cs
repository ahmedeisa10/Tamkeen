using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Tamkeen.Domain.Entities;

namespace Tamkeen.Infrastructure.Data.EntityTypeConfiguration
{
    public class SparePartConfig : IEntityTypeConfiguration<SparePartRequest>
    {
        public void Configure(EntityTypeBuilder<SparePartRequest> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.Price)
                .IsRequired()
                .HasPrecision(18, 2);


        }
    }
}
