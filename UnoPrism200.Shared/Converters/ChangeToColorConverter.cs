using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UnoPrism200.Shared.Converters
{
    public class ChangeToColorConverter : IValueConverter
    {
        public SolidColorBrush PlusColorBrush { get; set; }

        public SolidColorBrush MinusColorBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is float valueFloat)
            {
                if(valueFloat > 0)
                {
                    return PlusColorBrush;
                }
                else
                {
                    return MinusColorBrush;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
