using Domain.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RepairOrderConfiguration : IEntityTypeConfiguration<RepairOrder>
    {
        public void Configure(EntityTypeBuilder<RepairOrder> builder)
        {
            builder.ToTable("repair_orders");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasColumnName("id").ValueGeneratedNever();

            builder.Property(r => r.InstrumentId)
                .HasColumnName("instrument_id")
                .IsRequired();

            builder.Property(r => r.OrderDate)
                .HasColumnName("order_date")
                .IsRequired();

            builder.Property(r => r.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(r => r.Notes)
                .HasColumnName("notes")
                .HasMaxLength(1000);

            builder.Property(r => r.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(r => r.UpdatedAt)
                .HasColumnName("updated_at");
        }
    }
}
