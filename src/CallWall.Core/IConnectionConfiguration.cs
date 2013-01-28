using System;
using System.ComponentModel;

namespace CallWall
{
    public interface IConnectionConfiguration : INotifyPropertyChanged
    {
        bool IsEnabled { get; set; }
        string Name { get; }
        Uri Image { get; }
        object View { get; }
    }
}
