using DryIoc;
using Microsoft.Xaml.Interactivity;
using Prism.Container.Extensions;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Uno.Extensions;
using UnoPrism200.Extensions;
using UnoPrism200.Infrastructure.Events;
using Windows.UI.Xaml.Controls;

namespace UnoPrism200.Behaviors
{
    public class WebViewBehavior : Behavior<WebView>
    {
        private readonly IEventAggregator _eventAggregator;

        public WebViewBehavior()
        {
            _eventAggregator = PrismApplication.Current.GetEventAggregator();
        }

        protected override void OnAttached()
        {
            //todo : 여기서 사용 가능한 Container를 찾아서 IEventAggregator를 이용해서 Busy를 날려야함
            AssociatedObject.NavigationCompleted += AssociatedObject_NavigationCompleted;
            AssociatedObject.NavigationFailed += AssociatedObject_NavigationFailed;
            AssociatedObject.NavigationStarting += AssociatedObject_NavigationStarting;
        }

        private void AssociatedObject_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Debug.WriteLine("AssociatedObject_NavigationStarting");
            _eventAggregator.GetEvent<BusyEvent>()
                .Publish(new Infrastructure.EventArgs.BusyEventArgs
                {
                    Id = "NavigationWebView",
                    IsBusy = true,
                    Owner = GetType().Name,
                });
        }

        private void AssociatedObject_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            Debug.WriteLine("AssociatedObject_NavigationFailed");
            _eventAggregator.GetEvent<BusyEvent>()
                .Publish(new Infrastructure.EventArgs.BusyEventArgs
                {
                    Id = "NavigationWebView",
                    IsBusy = false,
                    Owner = GetType().Name,
                });
        }

        private void AssociatedObject_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            Debug.WriteLine("AssociatedObject_NavigationCompleted");
            _eventAggregator.GetEvent<BusyEvent>()
                .Publish(new Infrastructure.EventArgs.BusyEventArgs
                {
                    Id = "NavigationWebView",
                    IsBusy = false,
                    Owner = GetType().Name,
                });
        }

        protected override void OnDetaching()
        {
            AssociatedObject.NavigationCompleted -= AssociatedObject_NavigationCompleted;
            AssociatedObject.NavigationFailed -= AssociatedObject_NavigationFailed;
            AssociatedObject.NavigationStarting -= AssociatedObject_NavigationStarting;
        }
    }
}
