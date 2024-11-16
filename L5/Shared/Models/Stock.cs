using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; } // Relacja 1:1
    }
}
