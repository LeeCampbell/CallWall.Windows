using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Contract;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace CallWall.ProfileDashboard
{
    public sealed class DashboardModule : IModule, IDisposable
    {
        private readonly ITypeRegistry _typeRegistry;
        private readonly IRegionManager _regionManager;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly Func<IProfileActivatorAggregator> _profileActivatorAggregatorFactory;
        private readonly Func<IProfileDashboardView> _profileDashboardViewFactory;
        private readonly SingleAssignmentDisposable _activationSubscription = new SingleAssignmentDisposable();

        public DashboardModule(ITypeRegistry typeRegistry, IRegionManager regionManager, ISchedulerProvider schedulerProvider, 
            Func<IProfileActivatorAggregator> profileActivatorAggregatorFactory,
            Func<IProfileDashboardView> profileDashboardViewFactory)
        {
            _typeRegistry = typeRegistry;
            _regionManager = regionManager;
            _schedulerProvider = schedulerProvider;
            _profileActivatorAggregatorFactory = profileActivatorAggregatorFactory;
            _profileDashboardViewFactory = profileDashboardViewFactory;
        }

        public void Initialize()
        {
            RegisterTypes();

            ShowProfileViewOnProfileActivation();
        }

        private void RegisterTypes()
        {
            _typeRegistry.RegisterTypeAsTransient<Contact.IContactQueryAggregator, Contact.ContactQueryAggregator>();
            _typeRegistry.RegisterTypeAsTransient<Communication.ICommunicationQueryAggregator, Communication.CommunicationQueryAggregator>();
            _typeRegistry.RegisterTypeAsTransient<Pictures.IPictureQueryAggregator, Pictures.PictureQueryAggregator>();
            _typeRegistry.RegisterTypeAsTransient<Calendar.ICalendarQueryAggregator, Calendar.CalendarQueryAggregator>();

            _typeRegistry.RegisterTypeAsTransient<IProfileDashboardView, ProfileDashboardView>();
            _typeRegistry.RegisterTypeAsTransient<IProfileDashboard, ProfileDashboard>();
        }

        private void ShowProfileViewOnProfileActivation()
        {
            var activator = _profileActivatorAggregatorFactory();
            _activationSubscription.Disposable = activator.ProfileActivated()
                .ObserveOn(_schedulerProvider.Dispatcher)
                .Subscribe(ShowProfile);
        }

        private void ShowProfile(IProfile profile)
        {
            var windowRegion = _regionManager.Regions[RegionNames.WindowRegion];
            var view = _profileDashboardViewFactory();
            
            view.ViewModel.CloseCommand = new DelegateCommand(() => windowRegion.Remove(view));
            view.ViewModel.Load(profile);

            windowRegion.Add(view);
            windowRegion.Activate(view);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _activationSubscription.Dispose();
        }

        #endregion
    }
}
