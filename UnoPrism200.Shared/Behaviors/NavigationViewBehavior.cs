using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace UnoPrism200.Behaviors
{
    public class NavigationViewBehavior : Behavior<NavigationView>
    {
        protected override void OnAttached()
        {
            AssociatedObject.BackRequested += AssociatedObject_BackRequested;
        }

        private void AssociatedObject_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
        }

        protected override void OnDetaching()
        {
            AssociatedObject.BackRequested -= AssociatedObject_BackRequested;
        }


    }
}
