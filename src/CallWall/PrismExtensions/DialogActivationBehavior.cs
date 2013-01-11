using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Practices.Prism.Regions;

namespace CallWall.PrismExtensions
{
    /// <summary>
    /// Defines a behaviour that creates a Dialog to display the active view of the target <see cref="IRegion"/>.
    /// </summary>
    public abstract class DialogActivationBehavior : RegionBehavior
    {
        private readonly Dictionary<object, IWindow> _cache = new Dictionary<object, IWindow>();

        /// <summary>
        /// The key of this behaviour
        /// </summary>
        public const string BehaviorKey = "DialogActivation";

        /// <summary>
        /// Performs the logic after the behaviour has been attached.
        /// </summary>
        protected override void OnAttach()
        {
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
                    foreach (var view in e.NewItems)
                    {
                        Activate(view);    
                    }
                    
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var view in e.OldItems)
                    {
                        Deactivate(view);
                    }
                    break;
            }
        }

        private void Activate(object view)
        {
            var window = CreateWindow();
            window.Content = view;
            window.Closed += ContentDialogClosed;
            window.Show();
            _cache.Add(view, window);
        }

        private void Deactivate(object view)
        {
            IWindow window;
            if (_cache.TryGetValue(view, out window))
            {
                window.Close();
            }
        }

        private void ContentDialogClosed(object sender, System.EventArgs e)
        {
            var window = (IWindow) sender;
            var view = window.Content;
            if (_cache.TryGetValue(view, out window))
            {
                window.Closed -= ContentDialogClosed;
                window.Content = null;
                _cache.Remove(view);
            }
        }
    }
}