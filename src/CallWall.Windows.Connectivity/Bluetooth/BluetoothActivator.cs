using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CallWall.Contract;
using CallWall.Windows.Connectivity.Images;
using JetBrains.Annotations;

namespace CallWall.Windows.Connectivity.Bluetooth
{
    //TODO: Upgrade the BT contract to have kvps sent over the wire "ActivatedBy: mobile\t+4412345689\r\nProfileIncludes: email\tblah@blah.com\r\nProfileIncludes: email\t+6412345678"
    public sealed class BluetoothProfileActivator : IBluetoothProfileActivator, IDisposable
    {
        private readonly IBluetoothService _bluetoothService;
        private readonly IPersonalizationSettings _personalizationSettings;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ILogger _logger;
        private readonly IEventLoopScheduler _bluetoothEventLoop;
        private readonly IConnectableObservable<IProfile> _profileActivated;
        private readonly SerialDisposable _connection = new SerialDisposable();

        public BluetoothProfileActivator(IBluetoothService bluetoothService,
            IPersonalizationSettings personalizationSettings,
            ISchedulerProvider schedulerProvider,
            ILoggerFactory loggerFactory)
        {
            _bluetoothService = bluetoothService;
            _personalizationSettings = personalizationSettings;
            _schedulerProvider = schedulerProvider;
            _logger = loggerFactory.CreateLogger(GetType());
            _bluetoothEventLoop = _schedulerProvider.CreateEventLoopScheduler("BluetoothActivator");
            _logger.Verbose("BluetoothProfileActivator.ctor();");

            _profileActivated = _bluetoothService.IdentitiesActivated(_bluetoothEventLoop)
                .Retry()
                .Repeat()
                .Log(_logger, "IdentitiesActivated")
                .Select(Translate)
                .Publish();

            if (_bluetoothService.IsSupported && IsEnabled)
                _connection.Disposable = _profileActivated.Connect();
        }

        public bool IsEnabled
        {
            get { return _personalizationSettings.GetAsBool(LocalStoreKeys.BluetoothIsEnabled, false); }
            set
            {
                _personalizationSettings.SetAsBool(LocalStoreKeys.BluetoothIsEnabled, value);
                _connection.Disposable = value
                    ? _profileActivated.Connect()
                    : Disposable.Empty;
                OnPropertyChanged("IsEnabled");
            }
        }

        public IObservable<IProfile> ProfileActivated()
        {
            return _profileActivated.AsObservable();
        }

        private IProfile Translate(IEnumerable<string> phoneNumbers)
        {
            //TODO: When contract is updated, look up the source e.g. "mobile, email, homephone" for providerDesc, else fail over to BTProvDesc. -LC
            var ids = phoneNumbers.Select(pn => new PersonalIdentifier("mobile", pn, BluetoothProviderDescription.Instance));
            return new Profile(ids);
        }


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private sealed class BluetoothProviderDescription : IProviderDescription
        {
            public static readonly IProviderDescription Instance = new BluetoothProviderDescription();

            private BluetoothProviderDescription()
            { }

            public string Name { get { return "Bluetooth"; } }

            public Uri Image
            {
                get { return BluetoothImages.BluetoothIconUri; }
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
            _bluetoothEventLoop.Dispose();
        }
    }
}
