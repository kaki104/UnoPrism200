using Prism.Commands;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnoPrism200.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private object _selectedItem;
        /// <summary>
        /// Selected NavigationViewItem
        /// </summary>
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem ,value); }
        }

        public ShellViewModel()
        {
        }

        public ShellViewModel(IContainerProvider containerProvider)
            : base(containerProvider)
        {
            Title = "Shell Page";
        }
    }
}
