﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Infrastructure.EventArgs
{
    public class BusyEventArgs
    {
        public string Id { get; set; }

        public bool IsBusy { get; set; }

        public string Owner { get; set; }
    }
}
