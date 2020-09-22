using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Infrastructure.Models
{
    public class StockExchange
    {
        public object Close { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Volume { get; set; }
    }
}
