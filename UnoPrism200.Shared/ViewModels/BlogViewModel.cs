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
    public class BlogViewModel : ViewModelBase
    {
        private string _startUrl;
        /// <summary>
        /// Start Url
        /// </summary>
        public string StartUrl
        {
            get { return _startUrl; }
            set { SetProperty(ref _startUrl ,value); }
        }

        private int _viewCount;

        public ICommand FindCommand { get; set; }

        public BlogViewModel(IContainerProvider containerProvider) 
            : base(containerProvider)
        {
            Init();
        }

        private void Init()
        {
            Debug.WriteLine($"Created a {GetType().Name}");
            Title = "Blog";

            FindCommand = new DelegateCommand(OnFind);
            //ApplicationCommands.FindCommand.RegisterCommand(FindCommand);
        }

        private void OnFind()
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _viewCount++;
            Title = $"OnNavigatedTo {GetType().Name} {_viewCount}";
            StartUrl = "https://kaki104.tistory.com";
            //StartUrl = "https://m.cafe.daum.net/aspdotnet";

            ApplicationCommands.SetShellCommands(FindCommand);
        }

    }
}
