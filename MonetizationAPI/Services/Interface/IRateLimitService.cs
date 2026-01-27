using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonetizationAPI.Services.Interface
{
    public interface IRateLimitService
    {
        Task<bool> IsRequestAllowedAsync(Guid customerId, string endpoint);
    }
}
