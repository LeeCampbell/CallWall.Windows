using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CallWall.Windows.Google.Controls
{
    /// <summary>
    /// Provides extended control behaviors that seem to be missing from WPF.
    /// </summary>
    public static class Behaviors
    {
        #region DoubleClickCommand attached property
        /// <summary>
        /// Gets the double click command.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleClickCommandProperty);
        }

        /// <summary>
        /// Sets the double click command.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleClickCommandProperty, value);
        }

        /// <summary>
        /// The backing field for the DoubleClickCommand attached property. 
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(Behaviors), new PropertyMetadata(null, DoubleClickCommandChanged));

        /// <summary>
        /// Handles the changes to the DoubleClickCommand attached property.
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> that the attached property is targeting.</param>
        /// <param name="dpe">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DoubleClickCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs dpe)
        {
            var control = obj as Control;
            if (control != null)
            {
                control.MouseDoubleClick += (s, e) =>
                {
                    var command = dpe.NewValue as ICommand;
                    if (command != null)
                    {
                        var parameter = control.GetValue(Behaviors.DoubleClickCommandParameterProperty);
                        command.Execute(parameter);

                    }
                };
            }
        }

        #endregion

        #region DoubleClickCommandParameter attached property
        /// <summary>
        /// Gets the double click command parameter.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static object GetDoubleClickCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(DoubleClickCommandParameterProperty);
        }

        /// <summary>
        /// Sets the double click command parameter.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetDoubleClickCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(DoubleClickCommandParameterProperty, value);
        }

        /// <summary>
        /// The backing store for DoubleClickCommandParameter.
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommandParameter", typeof(object), typeof(Behaviors));
        #endregion

        #region SynchronizedSelectedItems attached property

        /// <summary>
        /// The backing field for the SynchronizedSelectedItems attached property. 
        /// </summary>
        public static readonly DependencyProperty SynchronizedSelectedItems = DependencyProperty.RegisterAttached(
            "SynchronizedSelectedItems",
            typeof(IList),
            typeof(Behaviors),
            new PropertyMetadata(null, OnSynchronizedSelectedItemsChanged));



        /// <summary>
        /// Gets the synchronized selected items.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The list that is acting as the sync list.</returns>
        /// <remarks>
        /// This attached property allows binding to the SelectedItems from implementations of <see cref="T:System.Windows.Controls.Primitives.Selector"/> such as <see cref="T:System.Windows.Controls.ListBox"/> or <see cref="T:System.Windows.Controls.ListView"/>
        /// <example>
        /// <![CDATA[
        /// <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        ///            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        ///            xmlns:pm="clr-namespace:ICAP.Relevance.Client.Module.Apollo.Calendar"
        ///            xmlns:inf="clr-namespace:ICAP.Relevance.Client.Infrastructure.AttachedProperties;assembly=ICAP.Relevance.Client.Infrastructure">
        ///     <DataTemplate DataType="{x:Type pm:CalendarSummaryModel}">
        ///			<ListBox x:Name="AppointmentList"
        ///         		 ItemsSource="{Binding Appointments}"
        ///         		 inf:Behaviors.SynchronizedSelectedItems="{Binding SelectedAppointments}"
        ///         		 SelectionMode="Extended" />
        ///     </DataTemplate>
        /// </ResourceDictionary>
        /// ]]>
        /// </example>
        /// </remarks>
        public static IList GetSynchronizedSelectedItems(DependencyObject dependencyObject)
        {
            return (IList)dependencyObject.GetValue(SynchronizedSelectedItems);
        }

        /// <summary>
        /// Sets the synchronized selected items.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value to be set as synchronized items.</param>
        /// <remarks>
        /// This attached property allows binding to the SelectedItems from implementations of <see cref="T:System.Windows.Controls.Primitives.Selector"/> such as <see cref="T:System.Windows.Controls.ListBox"/> or <see cref="T:System.Windows.Controls.ListView"/>
        /// <example>
        /// <![CDATA[
        /// <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        ///            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        ///            xmlns:pm="clr-namespace:ICAP.Relevance.Client.Module.Apollo.Calendar"
        ///            xmlns:inf="clr-namespace:ICAP.Relevance.Client.Infrastructure.AttachedProperties;assembly=ICAP.Relevance.Client.Infrastructure">
        ///     <DataTemplate DataType="{x:Type pm:CalendarSummaryModel}">
        ///			<ListBox x:Name="AppointmentList"
        ///         		 ItemsSource="{Binding Appointments}"
        ///         		 inf:Behaviors.SynchronizedSelectedItems="{Binding SelectedAppointments}"
        ///         		 SelectionMode="Extended" />
        ///     </DataTemplate>
        /// </ResourceDictionary>
        /// ]]>
        /// </example>
        /// </remarks>
        public static void SetSynchronizedSelectedItems(DependencyObject dependencyObject, IList value)
        {
            dependencyObject.SetValue(SynchronizedSelectedItems, value);
        }

        /// <summary>
        /// Called when synchronized selected items change.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSynchronizedSelectedItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            //Release previous attached property.
            if (e.OldValue != null)
            {
                SynchronizationManager synchronizer = GetSynchronizationManager(dependencyObject);
                synchronizer.StopSynchronizing();

                SetSynchronizationManager(dependencyObject, null);
            }

            var list = e.NewValue as IList;
            var selector = dependencyObject as Selector;

            // check that this property is an IList, and that it is being set on a ListBox
            if (list != null && selector != null)
            {
                SynchronizationManager synchronizer = GetSynchronizationManager(dependencyObject);
                if (synchronizer == null)
                {
                    synchronizer = new SynchronizationManager(selector);
                    SetSynchronizationManager(dependencyObject, synchronizer);
                }

                synchronizer.StartSynchronizingList();
            }
        }

        #region SynchronizationManager attached property
        private static readonly DependencyProperty SynchronizationManagerProperty = DependencyProperty.RegisterAttached(
            "SynchronizationManager", typeof(SynchronizationManager), typeof(Behaviors), new PropertyMetadata(null));
        private static SynchronizationManager GetSynchronizationManager(DependencyObject dependencyObject)
        {
            return (SynchronizationManager)dependencyObject.GetValue(SynchronizationManagerProperty);
        }
        private static void SetSynchronizationManager(DependencyObject dependencyObject, SynchronizationManager value)
        {
            dependencyObject.SetValue(SynchronizationManagerProperty, value);
        }
        #endregion

        #region Private classes
        /// <summary>
        /// A synchronization manager.
        /// </summary>
        private class SynchronizationManager
        {
            private readonly Selector _multiSelector;
            private TwoListSynchronizer _synchronizer;

            /// <summary>
            /// Initializes a new instance of the <see cref="SynchronizationManager"/> class.
            /// </summary>
            /// <param name="selector">The selector.</param>
            internal SynchronizationManager(Selector selector)
            {
                _multiSelector = selector;
            }

            /// <summary>
            /// Starts synchronizing the list.
            /// </summary>
            public void StartSynchronizingList()
            {
                IList list = GetSynchronizedSelectedItems(_multiSelector);

                if (list != null)
                {
                    _synchronizer = new TwoListSynchronizer(GetSelectedItemsCollection(_multiSelector), list);
                    _synchronizer.StartSynchronizing();
                }
            }

            /// <summary>
            /// Stops synchronizing the list.
            /// </summary>
            public void StopSynchronizing()
            {
                _synchronizer.StopSynchronizing();
            }

            public static IList GetSelectedItemsCollection(Selector selector)
            {
                var multiSelector = selector as MultiSelector;
                if (multiSelector != null)
                {
                    return multiSelector.SelectedItems;
                }
                var listBox = selector as ListBox;
                if (listBox != null)
                {
                    return listBox.SelectedItems;
                }
                throw new InvalidOperationException("Target object has no SelectedItems property to bind.");
            }

        }

        private class TwoListSynchronizer : IWeakEventListener
        {
            private readonly IList _masterList;
            private readonly IList _targetList;

            /// <summary>
            /// Initializes a new instance of the <see cref="TwoListSynchronizer"/> class.
            /// </summary>
            /// <param name="masterList">The master list.</param>
            /// <param name="targetList">The target list.</param>
            public TwoListSynchronizer(IList masterList, IList targetList)
            {
                _masterList = masterList;
                _targetList = targetList;
            }

            private delegate void ChangeListAction(IList list, NotifyCollectionChangedEventArgs e);

            /// <summary>
            /// Starts synchronizing the lists.
            /// </summary>
            public void StartSynchronizing()
            {
                ListenForChangeEvents(_masterList);
                ListenForChangeEvents(_targetList);


                //TODO: Should this update from the Control to the Bound Collection, or from the Bound Collection to the Control? -LC
                // Update the Target list from the Master list
                //SetListValuesFromSource(_masterList, _targetList);
                SetListValuesFromSource(_targetList, _masterList);
            }

            /// <summary>
            /// Stop synchronizing the lists.
            /// </summary>
            public void StopSynchronizing()
            {
                StopListeningForChangeEvents(_masterList);
                StopListeningForChangeEvents(_targetList);
            }

            /// <summary>
            /// Listens for change events on a list.
            /// </summary>
            /// <param name="list">The list to listen to.</param>
            private void ListenForChangeEvents(IList list)
            {
                var collection = list as INotifyCollectionChanged;
                if (collection != null)
                {
                    CollectionChangedEventManager.AddListener(collection, this);
                }
            }

            /// <summary>
            /// Stops listening for change events.
            /// </summary>
            /// <param name="list">The list to stop listening to.</param>
            private void StopListeningForChangeEvents(IList list)
            {
                var collection = list as INotifyCollectionChanged;
                if (collection != null)
                {
                    CollectionChangedEventManager.RemoveListener(collection, this);
                }
            }

            #region Private methods
            private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                var sourceList = sender as IList;

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        PerformActionOnAllLists(AddItems, sourceList, e);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        PerformActionOnAllLists(MoveItems, sourceList, e);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        PerformActionOnAllLists(RemoveItems, sourceList, e);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        PerformActionOnAllLists(ReplaceItems, sourceList, e);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        UpdateListsFromSource(sourceList);
                        break;
                    default:
                        break;
                }
            }

            private void AddItems(IList list, NotifyCollectionChangedEventArgs e)
            {
                int itemCount = e.NewItems.Count;

                for (int i = 0; i < itemCount; i++)
                {
                    int insertionPoint = e.NewStartingIndex + i;

                    if (insertionPoint > list.Count)
                    {
                        list.Add(e.NewItems[i]);
                    }
                    else
                    {
                        list.Insert(insertionPoint, e.NewItems[i]);
                    }
                }
            }

            private void RemoveItems(IList list, NotifyCollectionChangedEventArgs e)
            {
                int itemCount = e.OldItems.Count;

                // for the number of items being removed, remove the item from the Old Starting Index
                // (this will cause following items to be shifted down to fill the hole).
                for (int i = 0; i < itemCount; i++)
                {
                    list.RemoveAt(e.OldStartingIndex);
                }
            }

            private void MoveItems(IList list, NotifyCollectionChangedEventArgs e)
            {
                RemoveItems(list, e);
                AddItems(list, e);
            }

            private void ReplaceItems(IList list, NotifyCollectionChangedEventArgs e)
            {
                RemoveItems(list, e);
                AddItems(list, e);
            }

            private void PerformActionOnAllLists(ChangeListAction action, IList sourceList, NotifyCollectionChangedEventArgs collectionChangedArgs)
            {
                if (sourceList == _masterList)
                {
                    PerformActionOnList(_targetList, action, collectionChangedArgs);
                }
                else
                {
                    PerformActionOnList(_masterList, action, collectionChangedArgs);
                }
            }

            private void PerformActionOnList(IList list, ChangeListAction action, NotifyCollectionChangedEventArgs collectionChangedArgs)
            {
                StopListeningForChangeEvents(list);
                action(list, collectionChangedArgs);
                ListenForChangeEvents(list);
            }

            private void SetListValuesFromSource(IList sourceList, IList targetList)
            {
                StopListeningForChangeEvents(targetList);

                targetList.Clear();

                foreach (var item in sourceList)
                {
                    targetList.Add(item);
                }

                ListenForChangeEvents(targetList);
            }

            /// <summary>
            /// Makes sure that all synchronized lists have the same values as the source list.
            /// </summary>
            /// <param name="sourceList">The source list.</param>
            private void UpdateListsFromSource(IList sourceList)
            {
                if (sourceList == _masterList)
                {
                    SetListValuesFromSource(_masterList, _targetList);
                }
                else
                {
                    SetListValuesFromSource(_targetList, _masterList);
                }
            }
            #endregion

            #region IWeakEventListener Members
            /// <summary>
            /// Receives events from the centralized event manager.
            /// </summary>
            /// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager"/> calling this method.</param>
            /// <param name="sender">Object that originated the event.</param>
            /// <param name="e">Event data.</param>
            /// <returns>
            /// True if the listener handled the event. Always returns true.
            /// </returns>
            /// <remarks>
            /// It is considered an error by the <see cref="T:System.Windows.WeakEventManager"/> handling in WPF to register a listener for an event
            /// that the listener does not handle. Regardless, the method should return false if it receives an event that it does not recognize or handle.
            /// </remarks>
            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                //TODO: verify this is correct.
                //Only handle collection changes on the Dispatcher -- UNTESTED.
                //Dispatcher dispatcher = (Application.Current != null) ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
                //dispatcher.Invoke(
                //    new Action(() =>
                //        HandleCollectionChanged(sender as IList, e as NotifyCollectionChangedEventArgs)
                //    ),  DispatcherPriority.Background);

                HandleCollectionChanged(sender as IList, e as NotifyCollectionChangedEventArgs);
                return true;
            }

            #endregion
        }
        #endregion

        #endregion

        #region TextBoxPrompt attached property
        /// <summary>
        /// Gets the text box prompt.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string GetTextBoxPrompt(DependencyObject obj)
        {
            return (string)obj.GetValue(TextBoxPromptProperty);
        }

        /// <summary>
        /// Sets the text box prompt.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetTextBoxPrompt(DependencyObject obj, string value)
        {
            obj.SetValue(TextBoxPromptProperty, value);
        }

        /// <summary>
        /// The backing field for the TextBoxPrompt attached property. 
        /// </summary>
        public static readonly DependencyProperty TextBoxPromptProperty =
            DependencyProperty.RegisterAttached("TextBoxPrompt", typeof(string), typeof(Behaviors), new PropertyMetadata(null));
        #endregion

        #region ContextMenuOpenedCommand attached property

        /// <summary>
        /// This dependency property indicates the ContextMenuOpenedCommand.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(ContextMenu))]
        public static ICommand GetContextMenuOpenedCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(ContextMenuOpenedCommandProperty);
        }

        /// <summary>
        /// This dependency property indicates the ContextMenuOpenedCommand.
        /// </summary>
        public static void SetContextMenuOpenedCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(ContextMenuOpenedCommandProperty, value);
        }

        /// <summary>
        /// CommandOnContextMenuOpened Attached Dependency Property
        /// Calls the specified command each time the ContextMenu is opened.
        /// </summary>
        public static readonly DependencyProperty ContextMenuOpenedCommandProperty =
            DependencyProperty.RegisterAttached("ContextMenuOpenedCommand", typeof(ICommand), typeof(Behaviors),
            new PropertyMetadata(null, ContextMenuOpenedCommandPropertyChanged));

        /// <summary>
        /// Handles the property changed event for the LabelContent property
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        static private void ContextMenuOpenedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (ICommand)e.NewValue;
            var oldValue = (ICommand)e.OldValue;

            if (d as ContextMenu != null)
            {
                if (oldValue != null)
                    ((ContextMenu)d).Opened -= (s, ee) => oldValue.Execute(s);

                if (newValue != null)
                    ((ContextMenu)d).Opened += (s, ee) => newValue.Execute(s);
            }
        }
        #endregion

    }
}
