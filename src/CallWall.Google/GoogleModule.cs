﻿using CallWall.Contract.Communication;
using CallWall.Contract.Contact;
using CallWall.Google.Providers.Contacts;
using CallWall.PrismExtensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CallWall.Google
{
    public sealed class GoogleModule : IModule
    {
        private readonly IUnityContainer _container;

        public GoogleModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IAccountConfiguration, AccountConfiguration.GoogleAccountConfiguration>("GoogleAccountConfiguration", new ContainerControlledLifetimeManager());

            _container.RegisterType<Authorization.IOAuthUriFactory, Authorization.OAuthUriFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<Authorization.ISessionFactory, Authorization.SessionFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<Authorization.IGoogleOAuthService, Authorization.GoogleOAuthService>(new ContainerControlledLifetimeManager());
#if FAKE
            _container.RegisterType<Authorization.IGoogleAuthorization, Authorization.FakeGoogleAuthorization>(new ContainerControlledLifetimeManager());
#else
            _container.RegisterType<Authorization.IGoogleAuthorization, Authorization.GoogleAuthorization>(new ContainerControlledLifetimeManager());
#endif
            _container.RegisterType<Authorization.Login.IGoogleLoginView, Authorization.Login.GoogleLoginView>(new ContainerControlledLifetimeManager());
            _container.RegisterType<Authorization.Login.ILoginController, Authorization.Login.LoginController>(new ContainerControlledLifetimeManager());

            _container.RegisterType<AccountConfiguration.IGoogleAccountSetupView, AccountConfiguration.GoogleAccountSetupView>(new ContainerControlledLifetimeManager());
            _container.RegisterType<AccountConfiguration.IGoogleAccountSetupViewModel, AccountConfiguration.GoogleAccountSetupViewModel>(new ContainerControlledLifetimeManager());

#if !FAKE
            //Contacts
            _container.RegisterType<IGoogleContactProfileTranslator, GoogleContactProfileTranslator>(new ContainerControlledLifetimeManager());
            _container.RegisterComposite<IContactQueryProvider, IGoogleContactQueryProvider, GoogleContactQueryProvider>();
#endif

#if !FAKE
            //Mail
            _container.RegisterType<ICommunicationQueryProvider, Providers.Gmail.GmailCommunicationQueryProvider>("GmailCommunicationQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<Providers.Gmail.Imap.IImapClient, Providers.Gmail.Imap.ImapClient>(new TransientLifetimeManager());
#endif
            //Talk
            //Images??
            //Calendar

            var loginController = _container.Resolve<Authorization.Login.ILoginController>();
            loginController.Start();
        }
    }
}
