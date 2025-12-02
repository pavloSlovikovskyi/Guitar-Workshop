using Domain.Customers;
using Domain.InstrumentPassports;
using Domain.Instruments;
using Domain.RepairOrders;
using Domain.RepairOrdersServiceTypes;
using Domain.ServiceTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Instrument> Instruments => Set<Instrument>();
        public DbSet<RepairOrder> RepairOrders => Set<RepairOrder>();
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<RepairOrderServiceType> RepairOrderServiceTypes => Set<RepairOrderServiceType>();
        public DbSet<InstrumentPassport> InstrumentPassports => Set<InstrumentPassport>();
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            var instrumentIdConverter = new ValueConverter<InstrumentId, Guid>(
                id => id.Value,
                guid => new InstrumentId(guid));

            var repairOrderIdConverter = new ValueConverter<RepairOrderId, Guid>(
                id => id.Value,
                guid => new RepairOrderId(guid));

            var serviceTypeIdConverter = new ValueConverter<ServiceTypeId, Guid>(
                id => id.Value,
                guid => new ServiceTypeId(guid));

            var repairOrderServiceTypeOrderIdConverter = new ValueConverter<RepairOrderId, Guid>(
                id => id.Value,
                guid => new RepairOrderId(guid));

            var repairOrderServiceTypeServiceIdConverter = new ValueConverter<ServiceTypeId, Guid>(
                id => id.Value,
                guid => new ServiceTypeId(guid));

            var instrumentPassportIdConverter = new ValueConverter<InstrumentPassportId, Guid>(
                id => id.Value,
                guid => new InstrumentPassportId(guid));

            var customerIdConverter = new ValueConverter<CustomerId, Guid>(
                id => id.Value,
                guid => new CustomerId(guid));


            modelBuilder.Entity<Instrument>()
                .Property(i => i.Id)
                .HasConversion(instrumentIdConverter);

            modelBuilder.Entity<InstrumentPassport>()
                .Property(p => p.Id)
                .HasConversion(instrumentPassportIdConverter);

            modelBuilder.Entity<InstrumentPassport>()
                .Property(p => p.InstrumentId)
                .HasConversion(instrumentIdConverter);

            modelBuilder.Entity<RepairOrder>()
                .Property(r => r.Id)
                .HasConversion(repairOrderIdConverter);

            modelBuilder.Entity<RepairOrder>()
                .Property(r => r.InstrumentId)
                .HasConversion(instrumentIdConverter);

            modelBuilder.Entity<RepairOrderServiceType>()
                .Property(r => r.OrderId)
                .HasConversion(repairOrderServiceTypeOrderIdConverter);

            modelBuilder.Entity<RepairOrderServiceType>()
                .Property(r => r.ServiceId)
                .HasConversion(repairOrderServiceTypeServiceIdConverter);

            modelBuilder.Entity<ServiceType>()
                .Property(s => s.Id)
                .HasConversion(serviceTypeIdConverter);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Id)
                .HasConversion(customerIdConverter);

            modelBuilder.Entity<Instrument>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Instruments)
                .HasForeignKey(i => i.CustomerId)
                .IsRequired();

            modelBuilder.Entity<Instrument>()
                .HasOne(i => i.InstrumentPassport)
                .WithOne(p => p.Instrument)
                .HasForeignKey<InstrumentPassport>(p => p.InstrumentId)
                .IsRequired();

            modelBuilder.Entity<RepairOrder>()
                .HasOne(r => r.Instrument)
                .WithMany()
                .HasForeignKey(r => r.InstrumentId)
                .IsRequired();

            modelBuilder.Entity<RepairOrderServiceType>()
                .HasKey(x => new { x.OrderId, x.ServiceId });

            modelBuilder.Entity<RepairOrderServiceType>()
                .HasOne(rst => rst.Order)
                .WithMany()
                .HasForeignKey(rst => rst.OrderId)
                .IsRequired();

            modelBuilder.Entity<RepairOrderServiceType>()
                .HasOne(rst => rst.ServiceType)
                .WithMany(st => st.RepairOrderLinks)
                .HasForeignKey(rst => rst.ServiceId)
                .IsRequired();


        }
    }
}
