using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
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
        public static IObservable<TProperty> PropertyChanges<T, TProperty>(this T source, Expression<Func<T, TProperty>> property)
            where T : class, INotifyPropertyChanged
        {
            if (source == null) throw new ArgumentNullException("source");

            var propertyName = property.GetPropertyInfo().Name;
            var propertySelector = property.Compile();

            return Observable.Create<TProperty>(
                o => Observable.FromEventPattern
                         <PropertyChangedEventHandler, PropertyChangedEventArgs>
                         (
                             h => source.PropertyChanged += h,
                             h => source.PropertyChanged -= h
                         )
                         .Where(e => e.EventArgs.PropertyName == propertyName)
                         .Select(e => propertySelector(source))
                         .Subscribe(o));
        }

        /// <summary>
        /// Returns an observable sequence when <paramref name="source"/> raises <seealso cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source object. Type must implement <seealso cref="INotifyPropertyChanged"/>.</typeparam>
        /// <param name="source">The object to observe property changes on.</param>
        /// <returns>Returns an observable sequence with the source as its value. Values are produced each time the PropertyChanged event is raised.</returns>
        public static IObservable<T> AnyPropertyChanges<T>(this T source)
            where T : class, INotifyPropertyChanged
        {
            if (source == null) throw new ArgumentNullException("source");

            return Observable.FromEventPattern
                <PropertyChangedEventHandler, PropertyChangedEventArgs>
                (
                    h => source.PropertyChanged += h,
                    h => source.PropertyChanged -= h
                )
                .Select(_ => source);
        }

        public static IObservable<CollectionChangedData<TItem>> CollectionChanges<TItem>(this ObservableCollection<TItem> collection)
        {
            return CollectionChangesImp<ObservableCollection<TItem>, TItem>(collection);
        }

        public static IObservable<CollectionChangedData<TItem>> CollectionChanges<TItem>(
          this ReadOnlyObservableCollection<TItem> collection)
        {
            return CollectionChangesImp<ReadOnlyObservableCollection<TItem>, TItem>(collection);
        }

        private static IObservable<CollectionChangedData<TItem>> CollectionChangesImp<TCollection, TItem>(
           TCollection collection)
              where TCollection : IList<TItem>, INotifyCollectionChanged
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => collection.CollectionChanged += h,
                    h => collection.CollectionChanged -= h)
                .Select(e => new CollectionChangedData<TItem>(e.EventArgs));


        }


        //TODO: Allow the ability to push which property changed on underlying Item. (string.Empty for entire object)
        //TODO: Make Rx. Should allow filter by PropName (which would still push on string.Empty?)
        /// <summary>
        /// Returns an observable sequence of that represents modifications to a collection as they happen.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection items</typeparam>
        /// <param name="collection">The collection to observe.</param>
        /// <returns>Returns an observable sequence of <see cref="CollectionChangedData{T}"/>.</returns>
        public static IObservable<CollectionChangedData<TItem>> CollectionItemsChange<TItem>(
            this ObservableCollection<TItem> collection)
        {
            return ItemsPropertyChange<ObservableCollection<TItem>, TItem>(collection, _ => true);
        }

        public static IObservable<CollectionChangedData<TItem>> ItemsPropertyChange<TItem, TProperty>(
           this ObservableCollection<TItem> collection,
           Expression<Func<TItem, TProperty>> property)
            where TItem : INotifyPropertyChanged
        {
            var propertyName = property.GetPropertyInfo().Name;
            return ItemsPropertyChange<ObservableCollection<TItem>, TItem>(collection, propName => propName == propertyName);
        }

        public static IObservable<CollectionChangedData<TItem>> ItemsPropertyChange<TItem, TProperty>(
           this ReadOnlyObservableCollection<TItem> collection,
           Expression<Func<TItem, TProperty>> property)
            where TItem : INotifyPropertyChanged
        {
            var propertyName = property.GetPropertyInfo().Name;
            return ItemsPropertyChange<ReadOnlyObservableCollection<TItem>, TItem>(collection, propName => propName == propertyName);
        }

        private static IObservable<CollectionChangedData<TItem>> ItemsPropertyChange<TCollection, TItem>(
           TCollection collection,
           Predicate<string> isPropertyNameRelevant)
              where TCollection : IList<TItem>, INotifyCollectionChanged
        {
            return Observable.Create<CollectionChangedData<TItem>>(
                o =>
                {

                    var trackedItems = new List<INotifyPropertyChanged>();
                    PropertyChangedEventHandler onItemChanged =
                        (sender, e) =>
                        {
                            if (isPropertyNameRelevant(e.PropertyName))
                            {
                                var payload = new CollectionChangedData<TItem>((TItem)sender);
                                o.OnNext(payload);
                            }
                        };
                    Action<IEnumerable<TItem>> registerItemChangeHandlers =
                        items =>
                        {
                            foreach (var notifier in items.OfType<INotifyPropertyChanged>())
                            {
                                trackedItems.Add(notifier);
                                notifier.PropertyChanged += onItemChanged;
                            }
                        };
                    Action<IEnumerable<TItem>> unRegisterItemChangeHandlers =
                        items =>
                        {
                            foreach (var notifier in items.OfType<INotifyPropertyChanged>())
                            {
                                notifier.PropertyChanged -= onItemChanged;
                                trackedItems.Remove(notifier);
                            }
                        };
                    NotifyCollectionChangedEventHandler onCollectionChanged =
                        (sender, e) =>
                        {
                            if (e.Action == NotifyCollectionChangedAction.Reset)
                            {
                                foreach (var notifier in trackedItems)
                                {
                                    notifier.PropertyChanged -= onItemChanged;
                                }

                                var payload = new CollectionChangedData<TItem>(trackedItems, collection);
                                trackedItems.Clear();
                                registerItemChangeHandlers(collection);
                                o.OnNext(payload);
                            }
                            else
                            {
                                var payload = new CollectionChangedData<TItem>(e);
                                unRegisterItemChangeHandlers(payload.OldItems);
                                registerItemChangeHandlers(payload.NewItems);
                                o.OnNext(payload);
                            }
                        };

                    registerItemChangeHandlers(collection);
                    collection.CollectionChanged += onCollectionChanged;

                    return Disposable.Create(
                        () =>
                        {
                            collection.CollectionChanged -= onCollectionChanged;
                            unRegisterItemChangeHandlers(collection);
                        });
                });
        }
    }

}