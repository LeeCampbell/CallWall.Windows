using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;

namespace CallWall.PrismExtensions
{
    /// <summary>
    /// Defines the attached behaviour that keeps the items of the <see cref="T:System.Windows.Controls.Accordion"/> 
    /// host control in synchronization with the <see cref="T:Microsoft.Practices.Prism.Regions.IRegion"/>.
    /// This behaviour also makes sure that, if you activate a view in a region, the SelectedItem is set. 
    /// If you set the SelectedItem then this behaviour will also call Activate on the selected items.
    /// <remarks>
    /// When calling Activate on a view, you can only select a single active view at a time. 
    /// </remarks>
    /// </summary>
    public class AccordionItemsSourceSyncBehavior : RegionBehavior, IHostAwareRegionBehavior
    {
        /// <summary>
        /// Name that identifies the AccordionItemsSourceSyncBehavior behaviour in a collection of RegionsBehaviors.
        /// </summary>
        public static readonly string BehaviorKey = "AccordionItemsSourceSyncBehavior";
        private bool _updatingActiveViewsInHostControlSelectionChanged;
        private Accordion _hostControl;

        static AccordionItemsSourceSyncBehavior()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Windows.DependencyObject"/> that the <see cref="T:Microsoft.Practices.Prism.Regions.IRegion"/> is attached to.
        /// </summary>
        /// <value> A <see cref="T:System.Windows.DependencyObject"/> that the <see cref="T:Microsoft.Practices.Prism.Regions.IRegion"/> is attached to. </value>
        /// <remarks>
        /// For this behaviour, the host control must always be a <see cref="T:System.Windows.Controls.Accordion"/> or an inherited class.
        /// </remarks>
        public DependencyObject HostControl
        {
            get { return _hostControl; }
            set { _hostControl = value as Accordion; }
        }

        /// <summary>
        /// Starts to monitor the <see cref="T:Microsoft.Practices.Prism.Regions.IRegion"/> to keep it in synch with the items
        /// of the <see cref="P:CallWall.PrismExtensions.AccordionItemsSourceSyncBehavior.HostControl"/>.
        /// </summary>
        protected override void OnAttach()
        {
            if (_hostControl.ItemsSource != null || BindingOperations.GetBinding(_hostControl, ItemsControl.ItemsSourceProperty) != null)
                throw new InvalidOperationException("Accordion already has ItemsSource populated");
            SynchronizeItems();
            _hostControl.SelectionChanged += HostControlSelectionChanged;
            Region.ActiveViews.CollectionChanged += ActiveViewsCollectionChanged;
            Region.Views.CollectionChanged += ViewsCollectionChanged;
        }

        private void SynchronizeItems()
        {
            var list = _hostControl.Items.Cast<object>().ToList();
            foreach (var newItem in Region.Views)
                _hostControl.Items.Add(newItem);
            foreach (var view in list)
                Region.Add(view);
        }

        private void ViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newStartingIndex = e.NewStartingIndex;
                foreach (var insertItem in e.NewItems)
                    _hostControl.Items.Insert(newStartingIndex++, insertItem);
            }
            else
            {
                if (e.Action != NotifyCollectionChangedAction.Remove)
                    return;
                foreach (var removeItem in e.OldItems)
                    _hostControl.Items.Remove(removeItem);
            }
        }

        private void ActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_updatingActiveViewsInHostControlSelectionChanged)
                return;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (_hostControl.SelectedItem != null 
                    && _hostControl.SelectedItem != e.NewItems[0] 
                    && Region.ActiveViews.Contains(_hostControl.SelectedItem))
                    Region.Deactivate(_hostControl.SelectedItem);
                _hostControl.SelectedItem = e.NewItems[0];
            }
            else
            {
                if (e.Action != NotifyCollectionChangedAction.Remove || !e.OldItems.Contains(_hostControl.SelectedItem))
                    return;
                _hostControl.SelectedItem = null;
            }
        }

        private void HostControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _updatingActiveViewsInHostControlSelectionChanged = true;
                if (e.OriginalSource != sender)
                    return;
                foreach (var view in e.RemovedItems)
                {
                    if (Region.Views.Contains(view) && Region.ActiveViews.Contains(view))
                        Region.Deactivate(view);
                }
                foreach (var view in e.AddedItems)
                {
                    if (Region.Views.Contains(view) && !Region.ActiveViews.Contains(view))
                        Region.Activate(view);
                }
            }
            finally
            {
                _updatingActiveViewsInHostControlSelectionChanged = false;
            }
        }
    }
}