using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Infrastructure.EventArgs
{
    public class StockChangeEventArgs
    {
        public int Id { get; set; }

        public float Change { get; set; }
    }
}
