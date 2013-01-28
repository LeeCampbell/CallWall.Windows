using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CallWall.Contract;
using CallWall.Services;
using JetBrains.Annotations;

namespace CallWall.Activators
{
    public sealed class BluetoothProfileActivator : IBluetoothProfileActivator
    {
        private readonly IBluetoothService _bluetoothService;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ILogger _logger;
        private readonly IConnectableObservable<IProfile> _profileActivated;
        private readonly SerialDisposable _connection = new SerialDisposable();
        private bool _isEnabled;

        public BluetoothProfileActivator(IBluetoothService bluetoothService, ISchedulerProvider schedulerProvider, ILoggerFactory loggerFactory)
        {
            _bluetoothService = bluetoothService;
            _schedulerProvider = schedulerProvider;
            _logger = loggerFactory.CreateLogger();

            _profileActivated = _bluetoothService.IdentitiesActivated(_schedulerProvider.LongRunning)
                .Retry()
                .Repeat()
                .Log(_logger, "IdentitiesActivated")
                .Select(Translate)
                .Publish();
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                    return;

                _connection.Disposable = value 
                    ? _profileActivated.Connect() 
                    : Disposable.Empty;
                
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public IObservable<IProfile> ProfileActivated()
        {
            return _profileActivated.AsObservable();
        }

        private IProfile Translate(IEnumerable<string> phoneNumbers)
        {
            var ids = phoneNumbers.Select(pn => new PersonalIdentifier("mobile", pn, FakeProviderDescription.Instance));
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

        private sealed class FakeProviderDescription : IProviderDescription
        {
            public static readonly IProviderDescription Instance = new FakeProviderDescription();

            private FakeProviderDescription()
            {}

            public string Name { get { return "Test"; } }

            public Uri Image
            {
                get { return new Uri("pack://application:,,,/CallWall.FakeProvider;component/Images/Connectivity/Cloud_72x72.png"); }
            }
        }
    }
}
