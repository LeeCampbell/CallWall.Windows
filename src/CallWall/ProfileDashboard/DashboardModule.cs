using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace CallWall.ProfileDashboard
{
    public sealed class DashboardModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public DashboardModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #region Implementation of IModule

        public void Initialize()
        {
            _container.RegisterType<IProfileDashboardView, ProfileDashboardView>(new ContainerControlledLifetimeManager());

            var view = _container.Resolve<IProfileDashboardView>();
            _regionManager.AddToRegion(RegionNames.WindowRegion, view);
            view.ViewModel.CloseCommand = new DelegateCommand(() => _regionManager.Regions[RegionNames.WindowRegion].Remove(view));
            view.ViewModel.Activated.Subscribe(_ => _regionManager.Regions[RegionNames.WindowRegion].Activate(view));
        }

        #endregion
    }
}
