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
        private readonly ISettingsView _settingsView;
        private readonly IConnectivitySettingsView _connectivitySettingsView;
        private readonly IAccountSettingsView _accountSettingsView;
        private readonly IDemoView _demoView;
        private readonly TaskbarIcon _taskTaskbarIcon;

        public ToolbarController(IRegionManager regionManager,
            ISettingsView settingsView,
            IConnectivitySettingsView connectivitySettingsView,
            IAccountSettingsView accountSettingsView,
            IDemoView demoView)
        {
            _regionManager = regionManager;
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
            _taskTaskbarIcon.ContextMenu = new ContextMenu
                {
                    Items =
                        {
                            new MenuItem {Header = "Connection Settings...", Command = openConnectionSettingsCommand},
                            new MenuItem {Header = "Account Settings...", Command = openAccountSettingsCommand},
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

        private void ShutDownApp()
        {
            Application.Current.Shutdown(0);
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


        public void Dispose()
        {
            _taskTaskbarIcon.Dispose();
        }
    }
}
