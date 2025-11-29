using Domain.RepairOrdersServiceTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RepairOrderServiceTypeConfiguration : IEntityTypeConfiguration<RepairOrderServiceType>
    {
        public void Configure(EntityTypeBuilder<RepairOrderServiceType> builder)
        {
            builder.ToTable("repair_order_service_types");

            builder.HasKey(x => new { x.OrderId, x.ServiceId });
            builder.Property(x => x.OrderId).HasColumnName("order_id").IsRequired();
            builder.Property(x => x.ServiceId).HasColumnName("service_id").IsRequired();
        }
    }
}
