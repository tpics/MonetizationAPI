using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonetizationAPI.Entities
{
    public class Tier
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int MonthlyQuota { get; set; }
        public int RateLimitPerSecond { get; set; }
        public decimal Price { get; set; }
    }
}
