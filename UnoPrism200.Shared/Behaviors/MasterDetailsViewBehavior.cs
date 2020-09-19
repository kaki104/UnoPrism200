using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uno.Extensions.Specialized;
using UnoPrism200.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnoPrism200.Behaviors
{
    public class MasterDetailsViewBehavior : Behavior<MasterDetailsView>
    {
        private ListView _listView;

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
        }

        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_listView != null) return;
            FindListView(AssociatedObject);
        }

        private object FindListView(MasterDetailsView associatedObject)
        {
            var lists = associatedObject.FindChilds<ListView>();
            if(lists != null && lists.Any())
            {
                _listView = lists.First();
                SelectionMode = _listView.SelectionMode;
            }
            return _listView;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (_listView != null) return;
            FindListView(AssociatedObject);
        }

        #region SelectionMode

        public ListViewSelectionMode SelectionMode
        {
            get { return (ListViewSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(ListViewSelectionMode), 
                typeof(MasterDetailsViewBehavior), new PropertyMetadata(ListViewSelectionMode.Single, SelectionModeChanged));

        private static void SelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (MasterDetailsViewBehavior)d;
            behavior.SetSelectionMode(e.NewValue);
        }

        private void SetSelectionMode(object newValue)
        {
            if (_listView == null) 
            {
                if (FindListView(AssociatedObject) == null) return;
            };
            var newEnum = (ListViewSelectionMode)newValue;
            _listView.SelectionMode = newEnum;
        }
        #endregion
    }
}
