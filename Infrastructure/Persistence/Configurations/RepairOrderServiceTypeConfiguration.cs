using Domain.RepairOrdersServiceTypes;
using Domain.RepairOrders;
using Domain.ServiceTypes;
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

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .HasConversion(
                    v => v.Value,
                    v => new RepairOrderId(v))
                .IsRequired();

            builder.Property(x => x.ServiceId)
                .HasColumnName("service_id")
                .HasConversion(
                    v => v.Value,
                    v => new ServiceTypeId(v))
                .IsRequired();

            builder.HasOne<RepairOrder>()
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ServiceType>()
                .WithMany()
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
