using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace CallWall.Styles
{
    public sealed class AccordionItemToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var accoridion = value as AccordionItem;
            if (accoridion == null) return string.Empty;
            var index = accoridion.IndexInParent<Accordion>();
            return string.Format("{0}", index + 1);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}