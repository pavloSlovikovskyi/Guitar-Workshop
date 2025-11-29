using Domain.Instruments;
using Domain.RepairOrders;
using Domain.RepairOrdersServiceTypes;
using Domain.ServiceTypes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Instrument> Instruments => Set<Instrument>();
        public DbSet<RepairOrder> RepairOrders => Set<RepairOrder>();
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<RepairOrderServiceType> RepairOrderServiceTypes => Set<RepairOrderServiceType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
