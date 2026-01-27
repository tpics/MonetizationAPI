using MonetizationAPI;
using MonetizationAPI.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using MonetizationAPI.Services.Interface;

public class RateLimiterServiceTests
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _cache;

    public RateLimiterServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new AppDbContext(options);

        // Seed tier + customer
        _dbContext.Tiers.Add(new Tier
        {
            Id = Guid.NewGuid(),
            Name = "TestTier",
            MonthlyQuota = 2,
            RateLimitPerSecond = 1,
            Price = 0
        });
        _dbContext.SaveChanges();

        var tierId = _dbContext.Tiers.First().Id;
        _dbContext.Customers.Add(new Customer
        {
            Id = Guid.NewGuid(),
            Name = "TestCustomer",
            ApiKey = "test-key",
            TierId = tierId
        });
        _dbContext.SaveChanges();

        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    [Fact]
    public async Task ValidateRateLimitAndQuota()
    {
        var customerId = _dbContext.Customers.First().Id;
        var tier = _dbContext.Customers.Include(c => c.Tier).First().Tier;
        var rateLimiter = new RateLimitService(_cache, _dbContext);

        // Allowed
        var allowed1 = await rateLimiter.IsRequestAllowedAsync(customerId, "/api/LimitTest");
        allowed1.Should().BeTrue();

        // blocked (rate limit)
        var allowed2 = await rateLimiter.IsRequestAllowedAsync(customerId, "/api/LimitTest");
        allowed2.Should().BeFalse();
    }

}
