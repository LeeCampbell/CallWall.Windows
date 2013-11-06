using System;
using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Controls;
using CallWall.Settings;
using CallWall.Settings.Accounts;
using CallWall.Settings.Connectivity;
using CallWall.Settings.Demonstration;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace CallWall.Toolbar
{
    public class ToolbarController : IToolbarController
    {
        private readonly IRegionManager _regionManager;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly IPersonalizationSettings _settings;
        private readonly ISettingsView _settingsView;
        private readonly IConnectivitySettingsView _connectivitySettingsView;
        private readonly IAccountSettingsView _accountSettingsView;
        private readonly IDemoView _demoView;
        private readonly TaskbarIcon _taskTaskbarIcon;

        public ToolbarController(IRegionManager regionManager,
            ISchedulerProvider schedulerProvider,
            IPersonalizationSettings settings,
            ISettingsView settingsView,
            IConnectivitySettingsView connectivitySettingsView,
            IAccountSettingsView accountSettingsView,
            IDemoView demoView)
        {
            _regionManager = regionManager;
            _schedulerProvider = schedulerProvider;
            _settings = settings;
            _settingsView = settingsView;
            _connectivitySettingsView = connectivitySettingsView;
            _accountSettingsView = accountSettingsView;
            _demoView = demoView;
            _taskTaskbarIcon = new TaskbarIcon();
        }

        public void Start()
        {
            SetupViews();

            SetupToolbar();

            if (NoWindowShown())
            {
                ShowToolTip();
            }
            
        }

        private void ShowToolTip()
        {
            _taskTaskbarIcon.ShowBalloonTip("CallWall", "CallWall has started", BalloonIcon.Info);

            //Works
            _schedulerProvider.Dispatcher.Schedule(TimeSpan.FromSeconds(5), () => _taskTaskbarIcon.HideBalloonTip());
        }

        private bool NoWindowShown()
        {
            return true;    //HACK:
        }

        private void SetupToolbar()
        {
            _taskTaskbarIcon.Icon = ImageResources.CallWallIcon;

            //tb.CloseBalloon();
            //tb.CustomBalloon
            //tb.Dispose();
            //tb.HideBalloonTip();
            //tb.Icon;
            //tb.IconSource;
            //tb.IsTaskbarIconCreated
            //tb.LeftClickCommand
            //tb.MenuActivation
            //tb.PopupActivation
            //tb.ResetBalloonCloseTimer();
            //tb.ShowBalloonTip(..);
            //tb.ShowCustomBalloon();
            //tb.SupportsCustomToolTips
            _taskTaskbarIcon.ToolTipText = "CallWall\r\nIncoming call monitor";
            //tb.TrayBalloonTipClicked
            //tb.TrayBalloonTipClosed
            //tb.TrayBalloonTipShown
            //tb.TrayContextMenuOpen
            //tb.TrayLeftMouseDown
            //tb.TrayLeftMouseUp
            //tb.TrayMiddleMouseDown
            //tb.TrayMiddleMouseUp
            //tb.TrayMouseDoubleClick
            //tb.TrayMouseMove
            //tb.TrayPopup
            //tb.TrayPopupOpen
            //tb.TrayPopupResolved
            //tb.TrayRightMouseDown
            //tb.TrayRightMouseUp
            //tb.TrayToolTip
            //tb.TrayToolTipClose
            //tb.TrayToolTipOpen
            //tb.TrayToolTipResolved

            var openConnectionSettingsCommand = new DelegateCommand(OpenConnectionSettings);
            var openAccountSettingsCommand = new DelegateCommand(OpenAccountSettings);
            var shutDownAppCommand = new DelegateCommand(ShutDownApp);
            var logoutCommand = new DelegateCommand(Logout);
            _taskTaskbarIcon.ContextMenu = new ContextMenu
                {
                    Items =
                        {
                            new MenuItem {Header = "Connection Settings...", Command = openConnectionSettingsCommand},
                            new MenuItem {Header = "Account Settings...", Command = openAccountSettingsCommand},
                            new MenuItem {Header = "Log off all accounts", Command = logoutCommand},
                            new MenuItem {Header = "Exit", Command = shutDownAppCommand},
                        }
                };
        }
        
        private void SetupViews()
        {
            _settingsView.ViewModel.CloseCommand = new DelegateCommand(() => _regionManager.Regions[RegionNames.WindowRegion].Deactivate(_settingsView));
            _regionManager.AddToRegion(RegionNames.WindowRegion, _settingsView);

            var welcomeSettingRegion = _regionManager.Regions[ShellRegionNames.SettingsRegion];
            welcomeSettingRegion.Add(_connectivitySettingsView);
            welcomeSettingRegion.Add(_accountSettingsView);
            welcomeSettingRegion.Add(_demoView);

            _connectivitySettingsView.ViewModel.Closed += (s, e) => welcomeSettingRegion.Activate(_accountSettingsView);
            _accountSettingsView.ViewModel.Closed += (s, e) => welcomeSettingRegion.Activate(_demoView);
        }

        private void OpenConnectionSettings()
        {
            _regionManager.Regions[RegionNames.WindowRegion].Activate(_settingsView);
            _regionManager.Regions[ShellRegionNames.SettingsRegion].Activate(_connectivitySettingsView);
        }

        private void OpenAccountSettings()
        {
            _regionManager.Regions[RegionNames.WindowRegion].Activate(_settingsView);
            _regionManager.Regions[ShellRegionNames.SettingsRegion].Activate(_accountSettingsView);
        }

        private void Logout()
        {
            _settings.ClearAll();
        }

        private static void ShutDownApp()
        {
            Application.Current.Shutdown(0);
        }
        
        public void Dispose()
        {
            _taskTaskbarIcon.Dispose();
        }
    }
}
