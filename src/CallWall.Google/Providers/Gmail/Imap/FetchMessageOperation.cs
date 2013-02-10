using System;
using System.Globalization;
using CallWall.Contract.Communication;

namespace CallWall.Google.Providers.Gmail.Imap
{
    //http://tools.ietf.org/search/rfc3501#page-54
    internal sealed class FetchMessageOperation : ImapOperationBase
    {
        private readonly string _command;

        public FetchMessageOperation(string messageId, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _command = string.Format("FETCH {0} BODY.PEEK[HEADER.FIELDS (FROM TO Message-ID Subject Date)]", messageId);
        }
        protected override string Command
        {
            get { return _command; }
        }

        public GmailEmail ExtractMessage()
        {
            bool isDateSet, isSubjectSet, isDirectionSet;
            isDateSet = isSubjectSet = isDirectionSet = false;
            var date = DateTimeOffset.MinValue;
            string subject = null;
            //string body = null;
            var direction = MessageDirection.Outbound;
            foreach (var line in ResponseLines)
            {
                if (line.StartsWith("Date: "))
                {
                    date = DateTimeOffset.ParseExact(line.Substring(6), "ddd, dd MMM yyyy hh:mm:ss zzz", CultureInfo.InvariantCulture);
                    isDateSet = true;
                    continue;
                }
                if (line.StartsWith("Subject: "))
                {
                    subject = line.Substring(9);
                    isSubjectSet = true;
                    continue;
                }
                if (line.StartsWith("From: "))
                {
                    if (line.ToLower().Contains("lee.ryan.campbell@gmail.com"))
                    {
                        direction = MessageDirection.Outbound;
                        isDirectionSet = true;
                    }
                    continue;
                }
                if (line.StartsWith("From: "))
                {
                    if (line.ToLower().Contains("lee.ryan.campbell@gmail.com"))
                    {
                        direction = MessageDirection.Inbound;
                        isDirectionSet = true;
                    }
                                            continue;
                }
            }
            /*
[<--]Date: Thu, 27 Sep 2012 09:13:40 +0100
[<--]Message-ID: <CAPLQusCjGquLW-shZiFkSEWnDZkWO1j+Kc6_aGvtpJybqiRdFQ@mail.gmail.com>
[<--]Subject: Erynne's details
[<--]From: Lee Campbell <lee.ryan.campbell@gmail.com>
[<--]To: Marcus Whitworth <hello@marcuswhitworth.com>

             */
            if (isDateSet && isSubjectSet && isDirectionSet)
            {
                return new GmailEmail(date, direction, subject, null);
            }

            return null;
        }
    }
}