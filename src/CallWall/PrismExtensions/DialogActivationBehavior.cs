using System.Collections.Specialized;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;

namespace CallWall.PrismExtensions
{
    /// <summary>
    /// Defines a behaviour that creates a Dialog to display the active view of the target <see cref="IRegion"/>.
    /// </summary>
    public abstract class DialogActivationBehavior : RegionBehavior, IHostAwareRegionBehavior
    {
        /// <summary>
        /// The key of this behaviour
        /// </summary>
        public const string BehaviorKey = "DialogActivation";

        private IWindow _contentDialog;

        /// <summary>
        /// Gets or sets the <see cref="DependencyObject"/> that the <see cref="IRegion"/> is attached to.
        /// </summary>
        /// <value>A <see cref="DependencyObject"/> that the <see cref="IRegion"/> is attached to.
        /// This is usually a <see cref="FrameworkElement"/> that is part of the tree.</value>
        public DependencyObject HostControl { get; set; }

        /// <summary>
        /// Performs the logic after the behaviour has been attached.
        /// </summary>
        protected override void OnAttach()
        {
            //TODO: Where is the release of this event handle? -LC
            Region.ActiveViews.CollectionChanged += OnActiveViewsCollectionChanged;
        }

        /// <summary>
        /// Override this method to create an instance of the <see cref="IWindow"/> that 
        /// will be shown when a view is activated.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IWindow"/> that will be shown when a 
        /// view is activated on the target <see cref="IRegion"/>.
        /// </returns>
        protected abstract IWindow CreateWindow();

        private void OnActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    CloseContentDialog();
                    PrepareContentDialog(e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    CloseContentDialog();
                    break;
                default:
                    //throw new NotSupportedException(string.Format("An action of {0} is not supported for the ActiveViews Collection of a dialog region", e.Action));
                    break;
            }
        }

        private Style GetStyleForView()
        {
            return HostControl.GetValue(RegionPopupBehaviors.ContainerWindowStyleProperty) as Style;
        }

        private void PrepareContentDialog(object view)
        {
            _contentDialog = CreateWindow();
            _contentDialog.Content = view;
            _contentDialog.Owner = HostControl;
            _contentDialog.Closed += ContentDialogClosed;
            _contentDialog.Style = GetStyleForView();
            _contentDialog.Show();
        }

        private void CloseContentDialog()
        {
            if (_contentDialog != null)
            {
                _contentDialog.Closed -= ContentDialogClosed;
                _contentDialog.Close();
                _contentDialog.Content = null;
                _contentDialog.Owner = null;
            }
        }

        private void ContentDialogClosed(object sender, System.EventArgs e)
        {
            Region.Deactivate(_contentDialog.Content);
            CloseContentDialog();
        }
    }
}