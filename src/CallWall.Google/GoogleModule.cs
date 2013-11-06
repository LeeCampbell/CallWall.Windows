using System;
using CallWall.Contract;
using CallWall.Contract.Communication;
using CallWall.Contract.Contact;
using CallWall.Google.Authorization.Login;
using CallWall.Google.Providers.Contacts;
using Microsoft.Practices.Prism.Modularity;

namespace CallWall.Google
{
    public sealed class GoogleModule : IModule
    {
        private readonly ITypeRegistry _container;
        private readonly Func<ILoginController> _loginControllerFactory;

        public GoogleModule(ITypeRegistry container, Func<ILoginController> loginControllerFactory)
        {
            _container = container;
            _loginControllerFactory = loginControllerFactory;
        }

        public void Initialize()
        {
            _container.RegisterCompositeAsSingleton<IAccountConfiguration, AccountConfiguration.GoogleAccountConfiguration>();

            _container.RegisterTypeAsSingleton<Authorization.IOAuthUriFactory, Authorization.OAuthUriFactory>();
            _container.RegisterTypeAsSingleton<Authorization.ISessionFactory, Authorization.SessionFactory>();
            _container.RegisterTypeAsSingleton<Authorization.IGoogleOAuthService, Authorization.GoogleOAuthService>();
#if FAKE
            _container.RegisterTypeAsSingleton<Authorization.IGoogleAuthorization, Authorization.FakeGoogleAuthorization>();
#else
            _container.RegisterTypeAsSingleton<Authorization.IGoogleAuthorization, Authorization.GoogleAuthorization>();
#endif
            _container.RegisterTypeAsSingleton<Authorization.Login.IGoogleLoginView, Authorization.Login.GoogleLoginView>();
            _container.RegisterTypeAsSingleton<Authorization.Login.ILoginController, Authorization.Login.LoginController>();

            _container.RegisterTypeAsSingleton<AccountConfiguration.IGoogleAccountSetupView, AccountConfiguration.GoogleAccountSetupView>();
            _container.RegisterTypeAsSingleton<AccountConfiguration.IGoogleAccountSetupViewModel, AccountConfiguration.GoogleAccountSetupViewModel>();

#if !FAKE
            //Contacts
            _container.RegisterTypeAsTransient<IGoogleContactProfileTranslator, GoogleContactProfileTranslator>();
            _container.RegisterCompositeAsSingleton<IContactQueryProvider, IGoogleContactQueryProvider, GoogleContactQueryProvider>();
#endif

#if !FAKE
            //Mail
            _container.RegisterCompositeAsSingleton<ICommunicationQueryProvider, Providers.Gmail.GmailCommunicationQueryProvider>();
            _container.RegisterTypeAsSingleton<Providers.Gmail.Imap.IImapClient, Providers.Gmail.Imap.ImapClient>();
#endif
            //Talk
            //Images??
            //Calendar

            var loginController = _loginControllerFactory();
            loginController.Start();
        }
    }
}
