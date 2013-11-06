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
            _container.RegisterTypeAsSingleton<Authorization.IGoogleAuthorization, Authorization.GoogleAuthorization>();
            _container.RegisterTypeAsSingleton<IGoogleLoginView, GoogleLoginView>();
            _container.RegisterTypeAsSingleton<ILoginController, LoginController>();

            _container.RegisterTypeAsSingleton<AccountConfiguration.IGoogleAccountSetupView, AccountConfiguration.GoogleAccountSetupView>();
            _container.RegisterTypeAsSingleton<AccountConfiguration.IGoogleAccountSetupViewModel, AccountConfiguration.GoogleAccountSetupViewModel>();


            //Contacts
            _container.RegisterTypeAsTransient<IGoogleContactProfileTranslator, GoogleContactProfileTranslator>();
            _container.RegisterCompositeAsSingleton<IContactQueryProvider, IGoogleContactQueryProvider, GoogleContactQueryProvider>();

            //Mail
            _container.RegisterCompositeAsSingleton<ICommunicationQueryProvider, Providers.Gmail.GmailCommunicationQueryProvider>();
            _container.RegisterTypeAsTransient<Providers.Gmail.Imap.IImapClient, Providers.Gmail.Imap.ImapClient>();

            //TODO : Google Calendar
            //TODO : Talk --> (Hangouts / Google+ posts)
            //TODO : Picassa --> (Google+ Photos)
            //TODO : Youtube
            //TODO : +1s
            

            var loginController = _loginControllerFactory();
            loginController.Start();
        }
    }
}
