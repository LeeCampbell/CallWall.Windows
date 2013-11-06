using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace CallWall.Windows.Shell
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
                CallWallLogoSource = (ImageSource)conv.ConvertFrom(CallWallLogoPath);
                CallWallImageSource = (ImageSource)conv.ConvertFrom(CallWallIconPath);
                BirthdayImageSource = (ImageSource)conv.ConvertFrom(BirthdayPath);
            }
        }
        public static readonly string CallWallLogoPath = "pack://application:,,,/CallWall.Windows.Shell;component/Images/CallWallLogo.png";
        public static readonly string CallWallIconPath = "pack://application:,,,/CallWall.Windows.Shell;component/Resources/CallWall.ico";
        public static readonly string BirthdayPath = "pack://application:,,,/CallWall.Windows.Shell;component/Images/Birthday_64x64.png";
        public static readonly Icon CallWallIcon;
        public static readonly ImageSource CallWallLogoSource;
        public static readonly ImageSource CallWallImageSource;
        public static readonly ImageSource BirthdayImageSource;
    }
}