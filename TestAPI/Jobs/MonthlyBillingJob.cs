using MonetizationAPI;
using MonetizationAPI.Entities;
using Microsoft.EntityFrameworkCore;

public class MonthlyBillingJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MonthlyBillingJob(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            
            var monthlyUsage = await db.ApiUsages
                .Where(u => u.Timestamp >= firstDayOfMonth)
                .GroupBy(u => u.CustomerId)
                .Select(g => new { CustomerId = g.Key, RequestCount = g.Count() })
                .ToListAsync();

            foreach (var usage in monthlyUsage)
            {
                var customer = await db.Customers.Include(c => c.Tier)
                    .FirstOrDefaultAsync(c => c.Id == usage.CustomerId);

                if (customer == null) continue;

                var amount = customer.Tier.Price; 
                var billing = new Billing
                {
                    CustomerId = customer.Id,
                    RequestsThisMonth = usage.RequestCount,
                    Amount = amount,
                    BillingMonth = firstDayOfMonth,
                    CalculatedAt = DateTime.UtcNow
                };

                db.Add(billing);
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
