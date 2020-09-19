using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Infrastructure.Models
{
    public class NavigationViewHeader : BindableBase
    {
        private string _title;

        public string Title 
        { 
            get => _title; 
            set => SetProperty(ref _title ,value); 
        }

        public object ViewModel { get; set; }
    }
}
