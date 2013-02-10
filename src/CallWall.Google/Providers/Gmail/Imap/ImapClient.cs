using System;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using CallWall.Contract.Communication;

namespace CallWall.Google.Providers.Gmail.Imap
{
    public sealed class ImapClient : IImapClient
    {
        private readonly ILoggerFactory _loggerFactory;
        private const string Prefix = "CW0";
        private readonly ILogger _logger;
        private SslStream _imapSslStream;
        private StreamReader _imapSslStreamReader;
        private int _commandCount = 0;


        public ImapClient(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger();
        }

        public bool Connect(string sHost, int nPort)
        {
            bool result = false;
            try
            {
                var imapServer = new TcpClient(sHost, nPort);
                _imapSslStream = new SslStream(imapServer.GetStream(), false, ValidateServerCertificate, null);

                try
                {
                    _imapSslStream.AuthenticateAsClient(sHost, null, SslProtocols.Default, false);
                }
                catch (AuthenticationException authEx)
                {
                    _logger.Warn(authEx, "Authentication failed");
                    _imapSslStream.Dispose();
                    imapServer.Close();
                    return false;
                }
                _imapSslStreamReader = new StreamReader(_imapSslStream);
                var text = _imapSslStreamReader.ReadLine();
                if (text != null && text.StartsWith("* OK"))
                {
                    result = Capability();
                }
            }
            catch (IOException ioEx)
            {
                _logger.Warn(ioEx, "Failed to connect");
            }
            return result;
        }

        public bool Authenticate(string user, string accessToken)
        {
            var authenticate = new OAuthOperation(user, accessToken, _loggerFactory);
            return Execute(authenticate);
        }

        public bool SelectFolder(string folder)
        {
            var op = new SelectFolderOperation(folder, _loggerFactory);
            return Execute(op);
        }


        public IObservable<IMessage> FindEmailsFromOrTo(string emailAddress)
        {
            return Observable.Create<IMessage>(
                o =>
                {
                    var searchOp = new SearchFromOrToEmailOperation(emailAddress, _loggerFactory);
                    if (Execute(searchOp))
                    {
                        return searchOp.MessageIds()
                            .Reverse()
                            .Take(10)
                            .Select(LoadMessage)
                            .Concat()
                            .Where(msg => msg != null)
                            .Subscribe(o);
                    }
                    o.OnError(new IOException("IMAP search failed"));
                    return Disposable.Empty;
                });
        }

        private IObservable<GmailEmail> LoadMessage(string messageId)
        {
            return Observable.Create<GmailEmail>(
                o =>
                {
                    var op = new FetchMessageOperation(messageId, _loggerFactory);
                    if (Execute(op))
                    {
                        var email = op.ExtractMessage();
                        o.OnNext(email);
                        o.OnCompleted();
                    }
                    o.OnError(new IOException("Loading email failed"));
                    return Disposable.Empty;
                });
        }


        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            _logger.Warn("Certificate error: {0}", sslPolicyErrors);
            return false;
        }

        private bool Capability()
        {
            var operation = new CapabilityOperation(_loggerFactory);
            return Execute(operation);
        }

        private bool Execute(IIMapOperation command)
        {
            return command.Execute(
                string.Format("{0}{1}", Prefix, ++_commandCount),
                _imapSslStream,
                _imapSslStreamReader);
        }

        #region IDisposable implementation

        public void Dispose()
        {
            if (_imapSslStream != null)
            {
                _imapSslStream.Dispose();
                _imapSslStream = null;
            }
            if (_imapSslStreamReader != null)
            {
                _imapSslStreamReader.Dispose();
                _imapSslStreamReader = null;
            }
        }

        #endregion
    }
}
