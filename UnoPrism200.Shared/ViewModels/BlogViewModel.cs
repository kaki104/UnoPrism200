﻿using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using UnoPrism200.ViewModels;

namespace UnoPrism200.ViewModels
{
    public class BlogViewModel : ViewModelBase
    {
        public BlogViewModel(IContainerProvider containerProvider) 
            : base(containerProvider)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }

    }
}