using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonetizationAPI.Services.Interface
{
    public interface IApiUsageLogger
    {
        Task LogAsync(Guid customerId, string endpoint, bool success);
    }
}
