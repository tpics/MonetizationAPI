using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonetizationAPI;
using MonetizationAPI.Entities;

public class TestAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            
            var descriptors = services
                .Where(d => d.ServiceType.Name.Contains("DbContext") ||
                           (d.ServiceType.IsGenericType &&
                            d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestsDb");
            });

            
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted(); 
            db.Database.EnsureCreated();
            Seed(db);
        });
    }

    private static void Seed(AppDbContext db)
    {
        var tierId = Guid.NewGuid();
        var customerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var tier = new Tier
        {
            Id = tierId,
            Name = "Basic",
            MonthlyQuota = 5,
            RateLimitPerSecond = 2,
            Price = 9.99m
        };

        var customer = new Customer
        {
            Id = customerId,
            Name = "Test Customer",
            ApiKey = "test-api-key",
            TierId = tierId,   
            Tier = tier        
        };

        db.Tiers.Add(tier);
        db.Customers.Add(customer);
        db.SaveChanges();
    }
}
