using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CallWall
{
    public sealed class GrayscaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapSource bitmapSource = null;

            var uri = value as Uri;
            if (uri != null)
            {
                var imageSourceConverter = new ImageSourceConverter();
                var imageSource = imageSourceConverter.ConvertFrom(uri);
                bitmapSource = imageSource as BitmapSource;
            }

            if (bitmapSource == null)
            {
                bitmapSource = value as BitmapSource;
            }
            if (bitmapSource != null)
            {
                var converter = GetConverter(bitmapSource.Format);
                return converter.Convert(bitmapSource);
            }
            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public static IBitmapSourceGrayScaleConverter GetConverter(PixelFormat format)
        {
            if (format == PixelFormats.Bgra32 || format == PixelFormats.Bgr32)
                return BgrGrayScaleConverter.Instance;


            throw new NotSupportedException(string.Format("PixelFormat {0} is not supported", format.GetType().Name));
        }

        public static byte ToGray(byte red, byte green, byte blue)
        {
            int grayVal = (red + green + blue);

            if (grayVal != 0)
                grayVal = grayVal / 3;
            return (byte)grayVal;
        }
    }

    public interface IBitmapSourceGrayScaleConverter
    {
        BitmapSource Convert(BitmapSource source);
    }

    public sealed class BgrGrayScaleConverter : IBitmapSourceGrayScaleConverter
    {
        private const int Depth = 4;
        //private const int DpiX = 96;  //Maintain the source DPI
        //private const int DpiY = 96;
        private static readonly BitmapPalette _bitmapPalette = null;    //Only useful for index palettes.

        private BgrGrayScaleConverter()
        {}

        public static readonly IBitmapSourceGrayScaleConverter Instance = new BgrGrayScaleConverter();

        public BitmapSource Convert(BitmapSource source)
        {
            var orgPixels = new byte[source.PixelHeight * source.PixelWidth * Depth];
            var newPixels = new byte[orgPixels.Length];
            source.CopyPixels(orgPixels, source.PixelWidth * Depth, 0);
            for (int i = 3; i < orgPixels.Length; i += Depth)
            {
                var grayVal = GrayscaleConverter.ToGray(orgPixels[i - 3], orgPixels[i - 2], orgPixels[i - 1]);

                newPixels[i - 3] = grayVal;
                newPixels[i - 2] = grayVal;
                newPixels[i - 1] = grayVal;
                newPixels[i] = orgPixels[i]; //Set AlphaChannel
            }
            return BitmapSource.Create(source.PixelWidth, source.PixelHeight,
                                       source.DpiX, source.DpiY, //Not sure why we don't do this?
                                       //DpiX, DpiY, 
                                       PixelFormats.Bgra32, //} There seems to be no built in support for 256 gray values with 256alpha. Instead you
                                       _bitmapPalette,      //}     have to recreate the gray with RGB and maintain the alpha from the source.
                                       newPixels,
                                       source.PixelWidth * Depth);

        }
    }
}