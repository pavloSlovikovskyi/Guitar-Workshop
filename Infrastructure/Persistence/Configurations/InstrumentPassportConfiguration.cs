using Domain.InstrumentPassports;
using Domain.Instruments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class InstrumentPassportConfiguration : IEntityTypeConfiguration<InstrumentPassport>
    {
        public void Configure(EntityTypeBuilder<InstrumentPassport> builder)
        {
            builder.ToTable("instrument_passports");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("id")
                .HasConversion(
                    v => v.Value,
                    v => new InstrumentPassportId(v))
                .ValueGeneratedNever();

            builder.Property(p => p.InstrumentId)
                .HasColumnName("instrument_id")
                .HasConversion(
                    v => v.Value,
                    v => new InstrumentId(v))
                .IsRequired();

            builder.Property(p => p.IssueDate)
                .HasColumnName("issue_date")
                .IsRequired();

            builder.Property(p => p.Details)
                .HasColumnName("details")
                .HasMaxLength(1000);

            builder.HasOne<Instrument>()
                .WithOne()
                .HasForeignKey<InstrumentPassport>(p => p.InstrumentId);
        }
    }
}
