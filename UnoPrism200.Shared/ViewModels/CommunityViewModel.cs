using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using UnoPrism200.ViewModels;

namespace UnoPrism200.ViewModels
{
    public class CommunityViewModel : ViewModelBase
    {
        private int _viewCount;

        public ICommand CheckCommand { get; set; }

        public CommunityViewModel(IContainerProvider containerProvider) 
            : base(containerProvider)
        {
            Init();
        }

        private void Init()
        {
            Debug.WriteLine($"Created a {GetType().Name}");
            Title = "Community";

            CheckCommand = new DelegateCommand(OnCheck);
        }

        private void OnCheck()
        {
            
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ApplicationCommands.SetShellCommands(checkCommand: CheckCommand);

            _viewCount++;
            Title = $"OnNavigatedTo {GetType().Name} {_viewCount}";
        }

    }
}
