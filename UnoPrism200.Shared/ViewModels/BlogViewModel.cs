using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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

        public BlogViewModel(IContainerProvider containerProvider) 
            : base(containerProvider)
        {
            Debug.WriteLine($"Created a {GetType().Name}");
            Title = "Blog";
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _viewCount++;
            Title = $"OnNavigatedTo {GetType().Name} {_viewCount}";
            StartUrl = "https://kaki104.tistory.com";
        }

    }
}
