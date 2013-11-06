using System;
using System.Windows.Media.Imaging;

namespace CallWall.Shell.Images
{
    public abstract class ActionImages
    {
        public static readonly BitmapSource RightArrow;

        static ActionImages()
        {
            Ensure.PackUriIsRegistered();
            RightArrow = new BitmapImage(new Uri(@"pack://application:,,,/CallWall.Shell;component/Images/Actions/RightArrow_32x32.png"));
        }
    }
}
