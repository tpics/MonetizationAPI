using Microsoft.EntityFrameworkCore;
using MonetizationAPI.Entities;
namespace MonetizationAPI
{
    public class AppDbContext : DbContext
    {
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options) { }
        public DbSet<Customer> Customers {  get; set; }
        public DbSet<Tier> Tiers { get; set; }
        public DbSet<ApiUsage> ApiUsages { get; set; }
        public DbSet<Billing> Billings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tier>().HasData(
                new Tier
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Free",
                    MonthlyQuota = 100,
                    RateLimitPerSecond = 2,
                    Price = 0
                },
                new Tier
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Pro",
                    MonthlyQuota = 100000,
                    RateLimitPerSecond = 10,
                    Price = 50
                }
            );

            // --- Seed Customers ---
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = Guid.Parse("c1d2e3f4-5678-90ab-cdef-1234567890ab"),
                    Name = "User A",
                    ApiKey = "UserA-api-key",
                    TierId = Guid.Parse("11111111-1111-1111-1111-111111111111") // Free tier
                },
                new Customer
                {
                    Id = Guid.Parse("c1d2e3f4-5678-90ab-cdef-1231567074ab"),
                    Name = "User B",
                    ApiKey = "UserB-api-key",
                    TierId = Guid.Parse("22222222-2222-2222-2222-222222222222") // Pro tier
                }
            );
        }
    }
}
