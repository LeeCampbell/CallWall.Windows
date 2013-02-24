using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using CallWall.Google.AccountConfiguration;

namespace CallWall.Google.Authorization
{
    public delegate IObservable<string> RequestAuthorizationCode(Uri authorizationUri);

    public interface IGoogleAuthorization : INotifyPropertyChanged
    {
        AuthorizationStatus Status { get; }
        ReadOnlyCollection<GoogleResource> AvailableResourceScopes { get; }

        void RegisterAuthorizationCallback(RequestAuthorizationCode callback);
        IObservable<Unit> Authorize(IEnumerable<GoogleResource> resources);
        IObservable<string> RequestAccessToken(GoogleResource resource);
    }
}