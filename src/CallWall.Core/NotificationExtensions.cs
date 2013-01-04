using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace CallWall
{
    public static class NotificationExtensions
    {
        /// <summary>
        /// Returns an observable sequence of a property value when the source raises <seealso cref="INotifyPropertyChanged.PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="T">The type of the source object. Type must implement <seealso cref="INotifyPropertyChanged"/>.</typeparam>
        /// <typeparam name="TProperty">The type of the property that is being observed.</typeparam>
        /// <param name="source">The object to observe property changes on.</param>
        /// <param name="property">An expression that describes which property to observe.</param>
        /// <returns>Returns an observable sequence of property values when the property changes.</returns>
        public static IObservable<TProperty> WhenPropertyChanges<T, TProperty>(this T source, Expression<Func<T, TProperty>> property)
            where T : INotifyPropertyChanged
        {
            return Observable.Create<TProperty>(
                o =>
                {
                    var propertyName = property.GetPropertyInfo().Name;
                    var propertySelector = property.Compile();

                    return Observable.FromEventPattern
                        <PropertyChangedEventHandler, PropertyChangedEventArgs>
                        (
                            h => source.PropertyChanged += h,
                            h => source.PropertyChanged -= h
                        )
                        .Where(e => e.EventArgs.PropertyName == propertyName)
                        .Select(e => propertySelector(source))
                        .Subscribe(o);
                });

        }
    }
}