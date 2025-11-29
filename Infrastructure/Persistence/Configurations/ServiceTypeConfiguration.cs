using Domain.ServiceTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
    {
        public void Configure(EntityTypeBuilder<ServiceType> builder)
        {
            builder.ToTable("service_types");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id").ValueGeneratedNever();

            builder.Property(s => s.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(s => s.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            builder.Property(s => s.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}
