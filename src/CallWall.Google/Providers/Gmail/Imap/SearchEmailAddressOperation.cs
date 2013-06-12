using System;
using System.Collections.Generic;
using System.Linq;

namespace CallWall.Google.Providers.Gmail.Imap
{
    //https://developers.google.com/gmail/imap_extensions
    //https://support.google.com/mail/answer/7190?hl=en
    internal sealed class SearchOperation : ImapOperationBase
    {
        private readonly string _searchQuery;
        private const string Prefix = "* SEARCH ";
        private static readonly char[] SplitChars = new[] { ' ' };

        public SearchOperation(string searchQuery, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _searchQuery = searchQuery.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        protected override string Command
        {
            get { return string.Format("SEARCH X-GM-RAW \"{0}\"", _searchQuery); }
        }


        public IEnumerable<ulong> MessageIds()
        {
            using (Logger.Time("MessageIds()"))
            {
                var response = ResponseLines.First.Value;

                var messageIdString = string.Empty;
                if (response.StartsWith(Prefix))
                {
                    messageIdString = response.Substring(Prefix.Length);
                }

                return messageIdString.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ulong.Parse);
            }
        }
    }
}