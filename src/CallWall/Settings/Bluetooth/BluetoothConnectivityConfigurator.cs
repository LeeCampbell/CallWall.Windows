﻿using System;
using System.ComponentModel;

namespace CallWall.Settings.Bluetooth
{
    public sealed class BluetoothConnectivityConfigurator : IConnectivityConfigurator
    {
        private bool _isEnabled;
        private static readonly Uri _image;

        static BluetoothConnectivityConfigurator()
        {
            if (!UriParser.IsKnownScheme("pack"))
            {
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);
            }
            _image = new Uri("pack://application:,,,/CallWall;component/Settings.Bluetooth/Bluetooth_64x64.png");
        }

        public BluetoothConnectivityConfigurator()
        {
            
        }


        #region Implementation of IConnectivityConfigurator

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        public string Name
        {
            get { return "Bluetooth"; }
        }

        public Uri Image
        {
            get { return _image; }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}