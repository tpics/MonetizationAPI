using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonetizationAPI.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public Guid TierId { get; set; }
        public Tier Tier { get; set; } = null!;
    }
}
