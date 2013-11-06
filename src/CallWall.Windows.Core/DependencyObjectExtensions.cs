using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows;

namespace CallWall.Windows
{
    public static class DependencyObjectExtensions
    {
        public static IDisposable WhenPropertyChanges<T>(this DependencyObject source, DependencyProperty property, Action<T> action)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(property, property.OwnerType);
            if (dpd == null)
                throw new InvalidOperationException("Can not register change handler for this dependency property.");

            EventHandler handler = delegate { action((T)source.GetValue(property)); };
            dpd.AddValueChanged(source, handler);
            return Disposable.Create(() => dpd.RemoveValueChanged(source, handler));
        }
    }
}