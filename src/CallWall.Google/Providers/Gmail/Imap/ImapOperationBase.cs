using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Text;

namespace CallWall.Google.Providers.Gmail.Imap
{
    internal abstract class ImapOperationBase : IIMapOperation
    {
        private readonly LinkedList<string> _responseLines = new LinkedList<string>();
        private readonly ILogger _logger;
        private bool _hasExecuted;

        protected ImapOperationBase(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger();
        }

        protected abstract string Command { get; }

        protected LinkedList<string> ResponseLines
        {
            get { return _responseLines; }
        }

        public virtual bool Execute(string prefix, SslStream sendStream, StreamReader receiveStream)
        {
            if(_hasExecuted)
                throw new InvalidOperationException("Operations can not be executed more than once.");
            _hasExecuted = true;

            var wasSent = Send(prefix, sendStream);
            return wasSent 
                && Receive(prefix, receiveStream);
        }

        protected virtual string ToImapCommand(string prefix)
        {
            return string.Concat(new object[]
                                     {
                                         prefix, 
                                         " ", 
                                         Command,
                                         "\r\n"
                                     });
        }

        protected virtual bool Send(string prefix, SslStream sendStream)
        {
            var commandText = ToImapCommand(prefix);
            _logger.Trace("[-->] {0}", commandText);
            byte[] bytes = Encoding.ASCII.GetBytes(commandText.ToCharArray());

            var hasSent = false;
            try
            {
                sendStream.Write(bytes, 0, bytes.Length);
                hasSent = true;
            }
            catch (IOException ioEx)
            {
                _logger.Error(ioEx, "Error sending IMAP command");
            }
            catch (InvalidOperationException invEx)
            {
                _logger.Error(invEx, "Error sending IMAP command");
            }

            return hasSent;
        }

        protected bool Receive(string prefix, StreamReader receiveStream)
        {
            var isSuccess = false;
            try
            {
                var canExpectMoreLines = true;
                while (canExpectMoreLines)
                {
                    var line = receiveStream.ReadLine();
                    _logger.Trace("[<--]{0}", line);

                    if (line != null)
                    {
                        ResponseLines.AddLast(line);
                        if (IsSuccessLine(prefix, line))
                        {
                            canExpectMoreLines = false;
                            isSuccess = true;
                        }
                        else if (IsFailureLine(prefix, line))
                        {
                            canExpectMoreLines = false;
                        }
                    }
                }
            }
            catch (IOException ioEx)
            {
                _logger.Error(ioEx, "Error receiving IMAP response.");
                isSuccess = false;
            }
            return isSuccess;
        }

        protected virtual bool IsSuccessLine(string prefix, string responseLine)
        {
            return responseLine.StartsWith(prefix + " OK");
        }

        protected virtual bool IsFailureLine(string prefix, string responseLine)
        {
            return responseLine.StartsWith(prefix + " NO")
                   || responseLine.StartsWith(prefix + " BAD")
                   || responseLine.StartsWith("* BAD");
        }
    }
}
