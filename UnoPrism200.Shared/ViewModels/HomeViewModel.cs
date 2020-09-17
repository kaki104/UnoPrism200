using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnoPrism200.Infrastructure.Events;
using UnoPrism200.ViewModels;

namespace UnoPrism200.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private int _viewCount;

        public HomeViewModel(IContainerProvider containerProvider) 
            : base(containerProvider)
        {
            Init();
        }

        private void Init()
        {
            Debug.WriteLine($"Created a {GetType().Name}");
            Title = "Home";
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //ApplicationCommands.SetShellCommands();

            _viewCount++;
            Title = $"OnNavigatedTo {GetType().Name} {_viewCount}";
            if(_viewCount % 2 == 0)
            {
                EventAggregator.GetEvent<MessageEvent>()
                    .Publish(new Infrastructure.EventArgs.MessageEventArgs 
                    {
                        Id = _viewCount,
                        Message = $"It was called an even number of {_viewCount} times."
                    });
            }
        }
    }
}
