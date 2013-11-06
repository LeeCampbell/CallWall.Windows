using CallWall.Contract;
using CallWall.Contract.Calendar;
using CallWall.Contract.Communication;
using CallWall.Contract.Contact;
using CallWall.Contract.Picture;
using Microsoft.Practices.Prism.Modularity;

namespace CallWall.FakeProvider
{
    public sealed class FakeModule : IModule
    {
        private readonly ITypeRegistry _typeRegistry;

        public FakeModule(ITypeRegistry typeRegistry)
        {
            _typeRegistry = typeRegistry;
        }

        public void Initialize()
        {
            _typeRegistry.RegisterCompositeAsSingleton<IConnectionConfiguration, Connectivity.UsbConnectionConfiguration>();
            _typeRegistry.RegisterCompositeAsSingleton<IConnectionConfiguration, Connectivity.WifiDirectConnectionConfiguration>();
            _typeRegistry.RegisterCompositeAsSingleton<IConnectionConfiguration, Connectivity.CloudConnectionConfiguration>();

            _typeRegistry.RegisterCompositeAsSingleton<IAccountConfiguration, AccountConfiguration.MicrosoftAccountConfiguration>();
            _typeRegistry.RegisterCompositeAsSingleton<IAccountConfiguration, AccountConfiguration.FacebookAccountConfiguration>();
            _typeRegistry.RegisterCompositeAsSingleton<IAccountConfiguration, AccountConfiguration.LinkedInAccountConfiguration>();
            _typeRegistry.RegisterCompositeAsSingleton<IAccountConfiguration, AccountConfiguration.TwitterAccountConfiguration>();
            _typeRegistry.RegisterCompositeAsSingleton<IAccountConfiguration, AccountConfiguration.YahooAccountConfiguration>();
            _typeRegistry.RegisterCompositeAsSingleton<IAccountConfiguration, AccountConfiguration.GithubAccountConfiguration>();

            _typeRegistry.RegisterCompositeAsSingleton<IContactQueryProvider, Providers.FakeGoogleContactQueryProvider>();

            _typeRegistry.RegisterCompositeAsSingleton<ICommunicationQueryProvider, Providers.GmailCommunicationQueryProvider>();
            _typeRegistry.RegisterCompositeAsSingleton<ICommunicationQueryProvider, Providers.SmsCommunicationQueryProvider>();
            _typeRegistry.RegisterCompositeAsSingleton<ICommunicationQueryProvider, Providers.TwitterCommunicationQueryProvider>();

            _typeRegistry.RegisterCompositeAsSingleton<IPictureQueryProvider, Providers.SkydrivePictureQueryProvider>();
            _typeRegistry.RegisterCompositeAsSingleton<IPictureQueryProvider, Providers.FacebookPictureQueryProvider>();

            _typeRegistry.RegisterCompositeAsSingleton<ICalendarQueryProvider, Providers.GoogleCalendarQueryProvider>();
        }
    }
}
