using DryIoc;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.IO;
using System.Reflection;
using UnoPrism200.Controls;
using UnoPrism200.ControlViewModels;
using UnoPrism200.Helpers;
using UnoPrism200.Infrastructure.Interfaces;
using UnoPrism200.Infrastructure.Models;
using UnoPrism200.Infrastructure.Services;
using UnoPrism200.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;

namespace UnoPrism200
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            ConfigureFilters(global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory);

            InitializeComponent();

#if NETFX_CORE
            //Setting header text for User-Agent
            UserAgentHelper.SetDefaultUserAgent(
                "Mozilla/5.0 (Windows Phone 10.0; Android 6.0.1; Microsoft; Lumia 950) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Mobile Safari/537.36 Edge/15.14900");
#endif
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        protected override void OnSuspending(SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override UIElement CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDalSync>(() =>
            {
                var dal = new SqliteSyncDal();
                var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "MyData.db");
                if(dal.SetDatabaseConnection(path) == false)
                {
                    dal.CreateTable<Stock>();
                    dal.Insert(new Stock { Id = 1, Symbol = "MSFT" });
                    dal.Insert(new Stock { Id = 2, Symbol = "TSLA" });
                    dal.Insert(new Stock { Id = 3, Symbol = "NKLA" });
                    dal.Insert(new Stock { Id = 4, Symbol = "SEDG" });
                    dal.Insert(new Stock { Id = 5, Symbol = "NVDA" });
                    dal.Insert(new Stock { Id = 6, Symbol = "AAPL" });
                    dal.Insert(new Stock { Id = 7, Symbol = "AMD" });
                    dal.Insert(new Stock { Id = 8, Symbol = "INTC" });
                    dal.Insert(new Stock { Id = 9, Symbol = "AMZN" });
                    dal.CreateTable<Valuation>();
                    dal.Insert(new Valuation { Id = 1, StockId = 1, Price = 205.41m, Time = DateTime.Now });
                    dal.Insert(new Valuation { Id = 2, StockId = 2, Price = 419.62m, Time = DateTime.Now });
                    dal.Insert(new Valuation { Id = 3, StockId = 3, Price = 35.79m, Time = DateTime.Now });
                    dal.Insert(new Valuation { Id = 4, StockId = 4, Price = 196.12m, Time = DateTime.Now });
                    dal.Insert(new Valuation { Id = 5, StockId = 5, Price = 514.89m, Time = DateTime.Now });
                    dal.Insert(new Valuation { Id = 6, StockId = 6, Price = 115.36m, Time = DateTime.Now });
                    dal.Insert(new Valuation { Id = 7, StockId = 7, Price = 77.90m, Time = DateTime.Now });
                    dal.Insert(new Valuation { Id = 8, StockId = 8, Price = 49.41m, Time = DateTime.Now });
                    dal.Insert(new Valuation { Id = 9, StockId = 9, Price = 3102.97m, Time = DateTime.Now });
                }
                return dal;
            });

            containerRegistry.RegisterForNavigation<BlogView>();
            containerRegistry.RegisterForNavigation<CommunityView>();
            containerRegistry.RegisterForNavigation<StockView>();

            containerRegistry.RegisterDialog<MessageControl, MessageViewModel>();


        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                string viewName = viewType.FullName;
                if (viewName == null)
                {
                    return null;
                }

                if (viewName.EndsWith("View"))
                {
                    viewName = viewName.Substring(0, viewName.Length - 4);
                }

                if (viewName.EndsWith("Control"))
                {
                    viewName = viewName.Substring(0, viewName.Length - 7);
                }

                viewName = viewName.Replace(".Views.", ".ViewModels.");
                viewName = viewName.Replace(".Controls.", ".ControlViewModels.");
                string viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                string viewModelName = $"{viewName}ViewModel, {viewAssemblyName}";
                return Type.GetType(viewModelName);
            });
        }

        /// <summary>
        /// Configures global logging
        /// </summary>
        /// <param name="factory"></param>
        private static void ConfigureFilters(ILoggerFactory factory)
        {
            factory
                .WithFilter(new FilterLoggerSettings
                    {
                        { "Uno", LogLevel.Warning },
                        { "Windows", LogLevel.Warning },

						// Debug JS interop
						// { "Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug },

						// Generic Xaml events
						// { "Windows.UI.Xaml", LogLevel.Debug },
						// { "Windows.UI.Xaml.VisualStateGroup", LogLevel.Debug },
						// { "Windows.UI.Xaml.StateTriggerBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.UIElement", LogLevel.Debug },

						// Layouter specific messages
						// { "Windows.UI.Xaml.Controls", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },
						// { "Windows.Storage", LogLevel.Debug },

						// Binding related messages
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },

						// DependencyObject memory references tracking
						// { "ReferenceHolder", LogLevel.Debug },

						// ListView-related messages
						// { "Windows.UI.Xaml.Controls.ListViewBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.ListView", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.GridView", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.VirtualizingPanelLayout", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.NativeListViewBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.ListViewBaseSource", LogLevel.Debug }, //iOS
						// { "Windows.UI.Xaml.Controls.ListViewBaseInternalContainer", LogLevel.Debug }, //iOS
						// { "Windows.UI.Xaml.Controls.NativeListViewBaseAdapter", LogLevel.Debug }, //Android
						// { "Windows.UI.Xaml.Controls.BufferViewCache", LogLevel.Debug }, //Android
						// { "Windows.UI.Xaml.Controls.VirtualizingPanelGenerator", LogLevel.Debug }, //WASM
					}
                )
#if DEBUG
                .AddConsole(LogLevel.Debug);
#else
				.AddConsole(LogLevel.Information);
#endif
        }
    }
}
