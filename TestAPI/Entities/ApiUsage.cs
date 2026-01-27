using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonetizationAPI.Entities
{
    public class ApiUsage
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Endpoint { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public bool Success { get; set; }
    }
}
