using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using UnoPrism200.Infrastructure.EventArgs;

namespace UnoPrism200.Infrastructure.Events
{
    public class StockChangeEvent : PubSubEvent<StockChangeEventArgs>
    {
    }
}
