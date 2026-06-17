using GLMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Infrastructure.Data
{
    public class GlmsDbContext : DbContext
    {
        public GlmsDbContext(DbContextOptions<GlmsDbContext> options)
     : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Contracts)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Contract>()
                .HasMany(c => c.ServiceRequests)
                .WithOne(sr => sr.Contract)
                .HasForeignKey(sr => sr.ContractId);

            modelBuilder.Entity<ServiceRequest>()
    .Property(sr => sr.CostUSD)
    .HasPrecision(18, 2);

            modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.CostZAR)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}

