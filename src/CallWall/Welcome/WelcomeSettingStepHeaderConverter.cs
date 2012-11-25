using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace CallWall.Welcome
{
    public sealed class WelcomeSettingStepHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var accoridion = value as AccordionItem;
            if (accoridion == null) return string.Empty;
            var accordionDataContext = accoridion.DataContext;

            var parent = accoridion.FindParent<Accordion>();
            var index = parent.Items.IndexOf(accordionDataContext);
            return string.Format("Step {0}", index + 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}