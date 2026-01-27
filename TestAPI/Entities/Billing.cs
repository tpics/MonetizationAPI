using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonetizationAPI.Entities
{
    public class Billing
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int RequestsThisMonth { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillingMonth { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
