using System.Windows;
using System.Windows.Media;

namespace CallWall
{
    public static class UIElementExtensions
    {
        public static T FindParent<T>(this DependencyObject element) where T : DependencyObject
        {
            //Walk up the visual tree to the nearest T item.
            var parent = element as T;
            while ((parent == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element);
                parent = element as T;
            }
            return parent;
        }
    }
}