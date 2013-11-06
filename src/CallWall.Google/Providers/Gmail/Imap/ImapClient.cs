using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using CallWall.Contract.Communication;

namespace CallWall.Google.Providers.Gmail.Imap
{
    public sealed class ImapClient : IImapClient
    {
        #region Private fields
        private const string Prefix = "CW0";
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEventLoopScheduler _dedicatedScheduler;
        private readonly ILogger _logger;

        private SslStream _imapSslStream;
        private StreamReader _imapSslStreamReader;
        private int _commandCount;

        #endregion

        public ImapClient(ISchedulerProvider schedulerProvider, ILoggerFactory loggerFactory)
        {
            _dedicatedScheduler = schedulerProvider.CreateEventLoopScheduler("IMAP");
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public IObservable<bool> Connect(string sHost, int nPort)
        {
            return Observable.Start(() => ConnectSync(sHost, nPort), _dedicatedScheduler);
        }

        public IObservable<bool> Authenticate(string user, string accessToken)
        {
            return Observable.Start(() => AuthenticateSync(user, accessToken), _dedicatedScheduler);
        }

        public IObservable<bool> SelectFolder(string folder)
        {
            return Observable.Start(() => SelectFolderSync(folder), _dedicatedScheduler);
        }

        public IObservable<IList<ulong>> FindEmailIds(string query)
        {
            return Observable.Start(() => FindEmailIdsSync(query), _dedicatedScheduler)
                             .Log(_logger, string.Format("FindEmailIds('{0}')", query));
        }

        public IObservable<IMessage> FetchEmailSummaries(IEnumerable<ulong> messageIds, IEnumerable<string> fromAddresses)
        {
            _logger.Debug("FetchEmailSummaries({0})", string.Join(", ", messageIds));
            return messageIds
                .Select(id=>LoadMessage(id, fromAddresses))
                .Concat()
                .Where(msg => msg != null)
                .Log(_logger, "FetchEmailSummaries");
        }


        private IObservable<GmailEmail> LoadMessage(ulong messageId, IEnumerable<string> fromAddresses)
        {
            return Observable.Start(() => LoadMessageSync(messageId, fromAddresses), _dedicatedScheduler)
                             .Log(_logger, string.Format("LoadMessage({0})", messageId));
        }

        private bool ConnectSync(string sHost, int nPort)
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

        private bool AuthenticateSync(string user, string accessToken)
        {
            var authenticate = new OAuthOperation(user, accessToken, _loggerFactory);
            return Execute(authenticate);
        }

        private bool SelectFolderSync(string folder)
        {
            var op = new SelectFolderOperation(folder, _loggerFactory);
            return Execute(op);
        }

        private IList<ulong> FindEmailIdsSync(string query)
        {
            var searchOp = new SearchOperation(query, _loggerFactory);
            if (Execute(searchOp))
            {
                var msgIds = searchOp.MessageIds().ToArray();
                return msgIds;
            }
            throw new IOException("IMAP search failed");
        }

        private GmailEmail LoadMessageSync(ulong messageId, IEnumerable<string> fromAddresses)
        {
            var op = new FetchMessageOperation(messageId, fromAddresses, _loggerFactory);
            if (Execute(op))
            {
                var email = op.ExtractMessage();
                return email;
            }
            throw new IOException("Loading email failed");
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
            //TODO: Protect all public sequences with TakeUntil [isDisposed], so they don't try to schedule work once disposed.

            if (_dedicatedScheduler != null)
            {
                _dedicatedScheduler.Dispose();
            }
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
