using CallWall.Contract;
using CallWall.Windows.Connectivity.Bluetooth;
using CallWall.Windows.Connectivity.Settings.Bluetooth;
using Microsoft.Practices.Prism.Modularity;

namespace CallWall.Windows.Connectivity
{
    public sealed class BluetoothModule : IModule
    {
        private readonly ITypeRegistry _registry;

        public BluetoothModule(ITypeRegistry registry)
        {
            _registry = registry;
        }

        public void Initialize()
        {
            _registry.RegisterTypeAsSingleton<IBluetoothService, BluetoothService>();
            _registry.RegisterCompositeAsSingleton<IProfileActivator, IBluetoothProfileActivator, BluetoothProfileActivator>();

            //  Bluetooth Connectivity
            _registry.RegisterCompositeAsSingleton<IConnectionConfiguration, BluetoothConnectionConfiguration>();
            _registry.RegisterTypeAsTransient<IBluetoothSetupView, BluetoothSetupView>();
            _registry.RegisterTypeAsTransient<IBluetoothSetupViewModel, BluetoothSetupViewModel>();


            //_registry.RegisterComposite<IProfileActivator, IUsbIdentityActivator, UsbIdentityActivator>();
            //_registry.RegisterComposite<IProfileActivator, IWifiDirectIdentityActivator, WifiDirectIdentityActivator>();
            //_registry.RegisterComposite<IProfileActivator, ICloudIdentityActivator, CloudIdentityActivator>();
            //_registry.RegisterComposite<IProfileActivator, IIsdnIdentityActivator, IsdnIdentityActivator>();

        }
    }
}
