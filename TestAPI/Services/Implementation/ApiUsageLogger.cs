using MonetizationAPI;
using MonetizationAPI.Entities;
using MonetizationAPI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonetizationAPI.Services.Implementation
{
    public class ApiUsageLogger : IApiUsageLogger
    {
        private readonly AppDbContext _dbContext;

        public ApiUsageLogger(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogAsync(Guid customerId, string endpoint, bool success)
        {
            _dbContext.ApiUsages.Add(new ApiUsage
            {
                CustomerId = customerId,
                Endpoint = endpoint,
                Timestamp = DateTime.UtcNow,
                Success = success
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
