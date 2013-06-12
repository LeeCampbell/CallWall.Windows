using CallWall.Contract.Communication;
using CallWall.Contract.Contact;
using CallWall.Google.Providers.Contacts;
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
            _container.RegisterType<Authorization.IGoogleAuthorization, Authorization.GoogleAuthorization>(new ContainerControlledLifetimeManager());
            _container.RegisterType<Authorization.Login.IGoogleLoginView, Authorization.Login.GoogleLoginView>(new ContainerControlledLifetimeManager());
            _container.RegisterType<Authorization.Login.ILoginController, Authorization.Login.LoginController>(new ContainerControlledLifetimeManager());

            _container.RegisterType<AccountConfiguration.IGoogleAccountSetupView, AccountConfiguration.GoogleAccountSetupView>(new ContainerControlledLifetimeManager());
            _container.RegisterType<AccountConfiguration.IGoogleAccountSetupViewModel, AccountConfiguration.GoogleAccountSetupViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IGoogleContactProfileTranslator, GoogleContactProfileTranslator>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IContactQueryProvider, GoogleContactQueryProvider>("GoogleContactQueryProvider", new ContainerControlledLifetimeManager());
            //Contacts

            //Mail
            _container.RegisterType<ICommunicationQueryProvider, Providers.Gmail.GmailCommunicationQueryProvider>("GmailCommunicationQueryProvider", new ContainerControlledLifetimeManager());
            _container.RegisterType<Providers.Gmail.Imap.IImapClient, Providers.Gmail.Imap.ImapClient>(new TransientLifetimeManager());

            //Talk
            //Images??
            //Calendar

            var loginController = _container.Resolve<Authorization.Login.ILoginController>();
            loginController.Start();
        }
    }
}
