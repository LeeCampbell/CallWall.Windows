using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CallWall.Styles
{
    public sealed class AccordionItemBackgroundConverter : IValueConverter
    {
        private readonly BrushCollection _colorRamp = new BrushCollection();

        public BrushCollection ColorRamp
        {
            get { return _colorRamp; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var accoridion = value as AccordionItem;
            if (accoridion == null) return string.Empty;

            var index = accoridion.IndexInParent<Accordion>();
            return ColorRamp[index];
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }


        public sealed class BrushCollection
        : List<Brush>
        {

        }
    }
}