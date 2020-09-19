﻿using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uno.Extensions;
using UnoPrism200.Infrastructure.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace UnoPrism200.Behaviors
{
    /// <summary>
    /// NavigationView Behavior
    /// </summary>
    public class NavigationViewBehavior : Behavior<NavigationView>
    {
        protected override void OnAttached()
        {
            AssociatedObject.BackRequested += AssociatedObject_BackRequested;
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = AssociatedObject.SelectedItem as NavigationViewItem;
            if(selectedItem.Name == "SettingsNavPaneItem")
            {
                //Setting을 선택해도 여기옮
                //selectedItem.Name
                //"SettingsNavPaneItem"
                //selectedItem.Content
                //"Settings"
                return;
            }
            SelectedMenuItem = selectedItem == null
                ? null
                : MenuItems.FirstOrDefault(m => m.Name == selectedItem.Name);

            if(AssociatedObject.Header == null)
            {
                AssociatedObject.Header = new NavigationViewHeader
                {
                    Title = SelectedMenuItem.Content,
                    ViewModel = AssociatedObject.DataContext
                };
            }
            else
            {
                ((NavigationViewHeader)AssociatedObject.Header).Title = SelectedMenuItem.Content;
            }
        }

        private void AssociatedObject_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
        }

        protected override void OnDetaching()
        {
            AssociatedObject.BackRequested -= AssociatedObject_BackRequested;
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }

        #region MenuItems

        public IList<NavigationMenuItem> MenuItems
        {
            get { return (IList<NavigationMenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register("MenuItems", typeof(IList<NavigationMenuItem>), 
                typeof(NavigationViewBehavior), new PropertyMetadata(null, MenuItemsChanged));

        private static void MenuItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (NavigationViewBehavior)d;
            behavior.SetMenuItems();
        }

        #endregion

        private void SetMenuItems()
        {
            if (AssociatedObject == null) return;
            AssociatedObject.MenuItems.Clear();

            if (MenuItems == null) return;
            foreach (var item in MenuItems)
            {
                var icon = (Symbol)Enum.Parse(typeof(Symbol), item.Icon);
                var menu = new NavigationViewItem 
                {
                    Name = item.Name,
                    Content = item.Content,
                    Icon = new SymbolIcon(icon)
                };
                
                AssociatedObject.MenuItems.Add(menu);
            }
        }

        #region SelectedMenuItem

        public NavigationMenuItem SelectedMenuItem
        {
            get { return (NavigationMenuItem)GetValue(SelectedMenuItemProperty); }
            set { SetValue(SelectedMenuItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedMenuItemProperty =
            DependencyProperty.Register("SelectedMenuItem", typeof(NavigationMenuItem),
                typeof(NavigationViewBehavior), new PropertyMetadata(null, SelectedMenuItemChanged));

        private static void SelectedMenuItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (NavigationViewBehavior)d;
            behavior.SetSelectedMenuItem();
        }

        #endregion

        private void SetSelectedMenuItem()
        {
            if (SelectedMenuItem == null)
            { 
                AssociatedObject.SelectedItem = null; 
            }
            else
            {
                AssociatedObject.SelectedItem = AssociatedObject.MenuItems
                    .OfType<NavigationViewItem>()
                    .FirstOrDefault(m => m.Name == SelectedMenuItem.Name);
            }
        }
    }
}
