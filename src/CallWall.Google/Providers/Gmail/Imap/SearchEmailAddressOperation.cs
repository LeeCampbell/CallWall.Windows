using System.Collections.Generic;
using System.Linq;

namespace CallWall.Google.Providers.Gmail.Imap
{
    internal sealed class SearchEmailAddressOperation : ImapOperationBase
    {
        private const string Prefix = "* SEARCH ";
        private readonly string _emailAddress;

        public SearchEmailAddressOperation(string emailAddress, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _emailAddress = emailAddress;
        }

        protected override string Command
        {
            get { return string.Format("SEARCH X-GM-RAW \"{0}\"", _emailAddress); }
        }

        public IEnumerable<ulong> MessageIds()
        {
            using (Logger.Time("MessageIds()"))
            {
                var response = ResponseLines.First.Value;

                string messageIdString = string.Empty;
                if (response.StartsWith(Prefix))
                {
                    messageIdString = response.Substring(Prefix.Length);
                }

                return messageIdString.Split(' ')
                    .Select(ulong.Parse);
            }
        }
    }
}