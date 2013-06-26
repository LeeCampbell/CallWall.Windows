using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace CallWall
{
    public sealed class ImageResources
    {
        static ImageResources()
        {
            var streamResourceInfo = Application.GetResourceStream(new Uri(CallWallIconPath));
            if (streamResourceInfo != null)
            {
                var iconStream = streamResourceInfo.Stream;
                CallWallIcon = new Icon(iconStream);
            }

            var conv = new ImageSourceConverter();
            if (conv.CanConvertFrom(typeof(string)))
            {
                CallWallImageSource = (ImageSource)conv.ConvertFrom(CallWallIconPath);
            }
        }
        public static readonly string CallWallIconPath = "pack://application:,,,/CallWall.Shell;component/Resources/CallWall.ico";
        public static readonly Icon CallWallIcon;
        public static readonly ImageSource CallWallImageSource;
    }
}