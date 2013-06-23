using System;
using CallWall.Properties;
using Hardcodet.Wpf.TaskbarNotification;

namespace CallWall.Toolbar
{
    internal interface IToolbarController : IDisposable
    {
        void Start();
    }

    class ToolbarController : IToolbarController
    {
        private readonly TaskbarIcon _tb;

        public ToolbarController()
        {
            _tb = new TaskbarIcon();
        }

        public void Start()
        {
            //tb.Icon = new System.Drawing.Icon("CallWall.ico")
            _tb.Icon = Resources.CallWall;
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
            _tb.ToolTipText = "CallWall\r\nIncoming call monitor";
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
        }

        public void Dispose()
        {
            _tb.Dispose();
        }
    }
}
