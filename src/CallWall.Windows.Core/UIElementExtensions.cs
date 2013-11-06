using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CallWall.Windows
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

        public static int IndexInParent<TParent>(this FrameworkContentElement item)
            where TParent : ItemsControl
        {
            if (item == null) return -1;
            return IndexInParent<TParent>(item, item.DataContext);
        }

        public static int IndexInParent<TParent>(this FrameworkElement item)
            where TParent : ItemsControl
        {
            if (item == null) return -1;
            return IndexInParent<TParent>(item, item.DataContext);
        }

        private static int IndexInParent<TParent>(DependencyObject item, object dataContext)
            where TParent : ItemsControl
        {
            var parent = item.FindParent<TParent>();
            return parent.Items.IndexOf(dataContext);
        }
    }
}