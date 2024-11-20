using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class OrderProductKey
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not OrderProductKey other) return false;
            return OrderId == other.OrderId && ProductId == other.ProductId;
        }

        public override int GetHashCode()
        {
            return OrderId.GetHashCode() * ProductId.GetHashCode();
        }
    }
}
