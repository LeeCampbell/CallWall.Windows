using System.Collections.Generic;

namespace CallWall.Google.Providers.Gmail.Imap
{
    internal sealed class SearchFromOrToEmailOperation : ImapOperationBase
    {
        private readonly string _emailAddress;

        public SearchFromOrToEmailOperation(string emailAddress, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _emailAddress = emailAddress;
        }

        protected override string Command
        {
            //get { return string.Format("SEARCH X-GM-RAW \"to:{0} from:{0}\"", _emailAddress); }
            get { return string.Format("SEARCH X-GM-RAW \"{0}\"", _emailAddress); }
        }

        public IEnumerable<string> MessageIds()
        {
            var response = ResponseLines.First.Value;
            var prefix = "* SEARCH ";
            string messageIdString = string.Empty;
            if (response.StartsWith(prefix))
            {
                messageIdString = response.Substring(prefix.Length);
            }

            return messageIdString.Split(' ');
        }
    }
}