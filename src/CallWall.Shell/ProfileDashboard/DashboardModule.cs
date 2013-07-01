using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CallWall.Activators;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace CallWall.ProfileDashboard
{
    public sealed class DashboardModule : IModule, IDisposable
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly SingleAssignmentDisposable _activationSubscription = new SingleAssignmentDisposable();

        public DashboardModule(IUnityContainer container, IRegionManager regionManager, ISchedulerProvider schedulerProvider)
        {
            _container = container;
            _regionManager = regionManager;
            _schedulerProvider = schedulerProvider;
        }

        public void Initialize()
        {
            RegisterTypes();

            ShowProfileViewOnProfileActivation();
        }

        private void RegisterTypes()
        {
            _container.RegisterType<Contact.IContactQueryAggregator, Contact.ContactQueryAggregator>(new TransientLifetimeManager());
            _container.RegisterType<Communication.ICommunicationQueryAggregator, Communication.CommunicationQueryAggregator>(new TransientLifetimeManager());
            _container.RegisterType<Pictures.IPictureQueryAggregator, Pictures.PictureQueryAggregator>(new TransientLifetimeManager());
            _container.RegisterType<Calendar.ICalendarQueryAggregator, Calendar.CalendarQueryAggregator>(new TransientLifetimeManager());

            _container.RegisterType<IProfileDashboardView, ProfileDashboardView>(new TransientLifetimeManager());
            _container.RegisterType<IProfileDashboard, ProfileDashboard>(new TransientLifetimeManager());
        }

        private void ShowProfileViewOnProfileActivation()
        {
            var activator = _container.Resolve<IProfileActivatorAggregator>();
            _activationSubscription.Disposable = activator.ProfileActivated()
                .ObserveOn(_schedulerProvider.Dispatcher)
                .Subscribe(ShowProfile);
        }

        private void ShowProfile(Contract.IProfile profile)
        {
            var windowRegion = _regionManager.Regions[RegionNames.WindowRegion];
            var view = _container.Resolve<IProfileDashboardView>();
            
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
