using System;
using System.Text;

namespace CallWall.Windows.Google.Providers.Gmail.Imap
{
    internal sealed class OAuthOperation : ImapOperationBase
    {
        private readonly string _controlAChar = char.ConvertFromUtf32(1);
        private readonly string _command;
        private bool _hasFailed;

        public OAuthOperation(string user, string accessToken, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            var parameter = string.Format("user={0}{2}auth=Bearer {1}{2}{2}",
                                          user,
                                          accessToken,
                                          _controlAChar);

            _command = "AUTHENTICATE XOAUTH2 " + ToBase64String(parameter);
        }

        protected override string Command
        {
            get { return _command; }
        }

        public override bool Execute(string prefix, System.Net.Security.SslStream sendStream, System.IO.StreamReader receiveStream)
        {
            if (base.Execute(prefix, sendStream, receiveStream))
            {
                return true;
            }
            //Authorization has failed, we should challenge the failure to find out why it failed.
            _hasFailed = true;
            var wasSent = Send(prefix, sendStream);
            return wasSent
                   && Receive(prefix, receiveStream);
        }

        protected override string ToImapCommand(string prefix)
        {
            return _hasFailed
                ? "\r\n"
                : base.ToImapCommand(prefix);
        }

        protected override bool IsFailureLine(string prefix, string responseLine)
        {
            return (responseLine.StartsWith("+ ") && responseLine.EndsWith("=="))
                   || base.IsFailureLine(prefix, responseLine);
        }

        private static string ToBase64String(string source)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(source));
        }
    }
}