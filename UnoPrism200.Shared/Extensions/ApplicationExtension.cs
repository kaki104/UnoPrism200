using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml;

namespace UnoPrism200.Extensions
{
    public static class ApplicationExtension
    {
        public static IContainerProvider GetContainer(this Application application)
        {
            if(application is PrismApplication pa)
            {
                return pa.Container;
            }
            return null;
        }

        public static IEventAggregator GetEventAggregator(this Application application)
        {
            if (application is PrismApplication pa)
            {
                return pa.Container.Resolve<IEventAggregator>();
            }
            return null;
        }

        public static IRegionManager GetRegionManager(this Application application)
        {
            if (application is PrismApplication pa)
            {
                return pa.Container.Resolve<IRegionManager>();
            }
            return null;
        }

    }
}
