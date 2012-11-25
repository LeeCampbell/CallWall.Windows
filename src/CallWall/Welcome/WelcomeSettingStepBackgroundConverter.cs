using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace CallWall.Welcome
{
    public sealed class WelcomeSettingStepBackgroundConverter : IValueConverter
    {
        private static readonly string[] _colorMap = new string[] { "#806080", "#804080", "#802080", "#800080",  };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var accoridion = value as AccordionItem;
            if (accoridion == null) return string.Empty;
            var accordionDataContext = accoridion.DataContext;

            var parent = accoridion.FindParent<Accordion>();
            var index = parent.Items.IndexOf(accordionDataContext);
            return _colorMap[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}