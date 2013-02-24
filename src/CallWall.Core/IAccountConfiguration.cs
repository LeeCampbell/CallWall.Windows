using System;
using System.ComponentModel;

namespace CallWall
{
    public interface IAccountConfiguration : INotifyPropertyChanged
    {
        //TODO: Add a Status that allows us to expose any errors. eg. when user removes access to CallWall
        bool IsEnabled { get; set; }
        string Name { get; }
        Uri Image { get; }
        object View { get; }
    }
}