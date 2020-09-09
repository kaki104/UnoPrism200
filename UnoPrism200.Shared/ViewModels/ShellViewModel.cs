using Prism.Commands;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnoPrism200.Infrastructure.Models;

namespace UnoPrism200.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private NavigationMenuItem _selectedItem;
        /// <summary>
        /// Selected NavigationMenuItem
        /// </summary>
        public NavigationMenuItem SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem ,value); }
        }

        private IList<NavigationMenuItem> _menus;
        /// <summary>
        /// Menus
        /// </summary>
        public IList<NavigationMenuItem> Menus
        {
            get { return _menus; }
            set { SetProperty(ref _menus, value); }
        }

        public ShellViewModel()
        {
        }

        public ShellViewModel(IContainerProvider containerProvider)
            : base(containerProvider)
        {
            Title = "Shell Page";

            Menus = new List<NavigationMenuItem> 
            {
                new NavigationMenuItem{ Content = "홈", Icon = "Home"},
                new NavigationMenuItem{ Content = "블로그", Icon = "Like"},
                new NavigationMenuItem{ Content = "커뮤니티", Icon = "People"},
            };

            SelectedItem = Menus.First();
        }
    }
}
