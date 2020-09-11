using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnoPrism200.ViewModels;

namespace UnoPrism200.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private int _viewCount;

        public HomeViewModel(IContainerProvider containerProvider) 
            : base(containerProvider)
        {
            Debug.WriteLine($"Created a {GetType().Name}");
            Title = "Home";
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _viewCount++;
            Title = $"OnNavigatedTo {GetType().Name} {_viewCount}";

        }
    }
}
