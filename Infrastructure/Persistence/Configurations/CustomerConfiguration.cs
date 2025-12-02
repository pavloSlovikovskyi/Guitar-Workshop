using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("customers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasConversion(
                    v => v.Value,
                    v => new CustomerId(v))
                .ValueGeneratedNever();

            builder.Property(c => c.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .HasColumnName("updated_at");
        }
    }
}
