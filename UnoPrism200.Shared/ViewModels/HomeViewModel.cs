using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using UnoPrism200.ViewModels;

namespace UnoPrism200.Shared.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IContainerProvider containerProvider) 
            : base(containerProvider)
        {
        }
    }
}
