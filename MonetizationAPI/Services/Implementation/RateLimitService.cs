using MonetizationAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MonetizationAPI.Entities;
using MonetizationAPI;

public class RateLimitService : IRateLimitService
{
    private readonly IMemoryCache _cache;
    private readonly AppDbContext _dbContext;

    public RateLimitService(IMemoryCache cache, AppDbContext dbContext)
    {
        _cache = cache;
        _dbContext = dbContext;
    }

    public async Task<bool> IsRequestAllowedAsync(Guid customerId, string endpoint)
    {
        var customer = await _dbContext.Customers.Include(c => c.Tier).FirstOrDefaultAsync(c => c.Id == customerId);
        if (customer == null) return false;

        // Rate limit per second
        #region 
        var secondKey = $"rate:{customerId}:{endpoint}:{DateTime.UtcNow:yyyyMMddHHmmss}";
        var count = _cache.GetOrCreate(secondKey, e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
            return 0;
        });

        if (count >= customer.Tier.RateLimitPerSecond) return false;
        _cache.Set(secondKey, count + 1);
        #endregion

        // Monthly quota
        #region 
        var monthKey = $"quota:{customerId}:{DateTime.UtcNow:yyyyMM}";
        var usageCount = _cache.GetOrCreate(monthKey, e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30);
            return 0;
        });

        if (usageCount >= customer.Tier.MonthlyQuota) return false;
        _cache.Set(monthKey, usageCount + 1);
        #endregion
        return true;
    }
}
