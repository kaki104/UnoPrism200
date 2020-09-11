﻿using Prism.Commands;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnoPrism200.Infrastructure.Consts;
using UnoPrism200.Infrastructure.EventArgs;
using UnoPrism200.Infrastructure.Events;
using UnoPrism200.Infrastructure.Models;
using UnoPrism200.Views;

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
        private readonly IDialogService _dialogService;

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

        public ShellViewModel(IContainerProvider containerProvider,
            IDialogService dialogService)
            : base(containerProvider)
        {
            Title = "Shell Page";
            _dialogService = dialogService;
            Menus = new List<NavigationMenuItem> 
            {
                new NavigationMenuItem{ Name = "Home", Content = "홈", Icon = "Home", Path = "HomeView"},
                new NavigationMenuItem{ Name = "Blog", Content = "블로그", Icon = "Like", Path = "BlogView"},
                new NavigationMenuItem{ Name = "Community", Content = "커뮤니티", Icon = "People", Path = "CommunityView"},
            };

            SelectedItem = Menus.First();

            //RegionManager.RequestNavigate(Regions.CONTENT_REGION, SelectedItem.Path);
            RegionManager.RegisterViewWithRegion(Regions.CONTENT_REGION, typeof(HomeView));

            EventAggregator.GetEvent<MessageEvent>()
                .Subscribe(ReceivedMessageEvent);

            PropertyChanged += ShellViewModel_PropertyChanged;

        }

        private void ReceivedMessageEvent(MessageEventArgs obj)
        {
            _dialogService.ShowDialog("MessageControl", 
                new DialogParameters($"message={obj.Message}&id={obj.Id}"),
                callback => 
                {
                    switch (callback.Result)
                    {
                        case ButtonResult.Cancel:
                            break;
                        case ButtonResult.None:
                            break;
                        case ButtonResult.OK:
                            break;
                    }
                });
        }

        private void ShellViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(SelectedItem):
                    if (SelectedItem == null) return;
                    RegionManager.RequestNavigate(Regions.CONTENT_REGION, SelectedItem.Path);
                    break;
            }
        }
    }
}
