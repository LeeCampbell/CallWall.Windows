using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;

namespace CallWall.ProfileDashboard
{
    public sealed class DashboardCollection<T> : INotifyPropertyChanged, IDisposable
    {
        private readonly ObservableCollection<T> _items = new ObservableCollection<T>();
        private readonly ReadOnlyObservableCollection<T> _roItems;
        private readonly IDisposable _subscription;
        private ViewModelStatus _status = ViewModelStatus.Processing;

        public DashboardCollection(IObservable<T> inputSequence)
        {
            _roItems = new ReadOnlyObservableCollection<T>(_items);
            _subscription = inputSequence.Subscribe(
                i=>_items.Add(i),
                ex => Status = ViewModelStatus.Error(ex.Message),
                () => Status = ViewModelStatus.Idle);
        }

        public ReadOnlyObservableCollection<T> Items
        {
            get { return _roItems; }
        }

        public ViewModelStatus Status
        {
            get { return _status; }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public void Dispose()
        {
            _subscription.Dispose();
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

    }
}