using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using UnoPrism200.Infrastructure.Interfaces;

namespace UnoPrism200.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        private readonly IDalSync _dal;

        public StockViewModel(IContainerProvider containerProvider,
            IDalSync dal) 
            : base(containerProvider)
        {
            _dal = dal;
        }
    }
}
