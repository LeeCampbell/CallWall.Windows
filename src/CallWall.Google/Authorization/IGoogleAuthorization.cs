using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CallWall.Google.AccountConfiguration;

namespace CallWall.Google.Authorization
{
    public delegate IObservable<string> RequestAuthorizationCode(Uri authorizationUri);

    public interface IGoogleAuthorization : INotifyPropertyChanged
    {
        AuthorizationStatus Status { get; }
        ReadOnlyCollection<GoogleResource> AvailableResourceScopes { get; }

        void RegisterAuthorizationCallback(RequestAuthorizationCode callback);
        IObservable<string> RequestAccessToken();
    }
}