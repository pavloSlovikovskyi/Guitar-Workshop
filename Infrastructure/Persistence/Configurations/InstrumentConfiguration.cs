using Domain.Customers;
using Domain.Instruments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder.ToTable("instruments");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .HasColumnName("id")
                .HasConversion(
                    v => v.Value,
                    v => new InstrumentId(v))
                .ValueGeneratedNever();

            builder.Property(i => i.Model)
                .HasColumnName("model")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.SerialNumber)
                .HasColumnName("serial_number")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.RecieveDate)
                .HasColumnName("recieve_date")
                .IsRequired();

            builder.Property(i => i.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(i => i.CustomerId)
                .HasColumnName("customer_id")
                .HasConversion(
                    v => v.Value,
                    v => new CustomerId(v))
                .IsRequired();

            builder.Property(i => i.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(i => i.UpdatedAt)
                .HasColumnName("updated_at");

            builder.HasOne<Domain.InstrumentPassports.InstrumentPassport>()
                .WithOne()
                .HasForeignKey<Domain.InstrumentPassports.InstrumentPassport>(p => p.InstrumentId);
        }
    }
}
