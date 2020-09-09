using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace UnoPrism200.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    {
        private string _title;
        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected IContainerProvider ContainerProvider { get; }

        protected IEventAggregator EventAggregator { get; }

        protected IRegionManager RegionManager { get; }

        public ViewModelBase()
        {
        }

        public ViewModelBase(IContainerProvider containerProvider)
            :this()
        {
            ContainerProvider = containerProvider;
            EventAggregator = ContainerProvider.Resolve<IEventAggregator>();
            RegionManager = ContainerProvider.Resolve<IRegionManager>();
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void Destroy()
        {
            
        }   
    }
}
