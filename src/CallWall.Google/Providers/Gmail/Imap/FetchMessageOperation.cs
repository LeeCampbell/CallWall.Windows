using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CallWall.Contract.Communication;

namespace CallWall.Google.Providers.Gmail.Imap
{
    //http://tools.ietf.org/search/rfc3501#page-54
    internal sealed class FetchMessageOperation : ImapOperationBase
    {
        private readonly string _command;

        public FetchMessageOperation(ulong messageId, ILoggerFactory loggerFactory)
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
            bool isDateSet, isSubjectSet;
            isDateSet = isSubjectSet = false;
            var date = DateTimeOffset.MinValue;
            string subject = null;
            var direction = MessageDirection.Inbound;

            var kvp = new Dictionary<string, string>();
            var lastKey = string.Empty;
            foreach (var line in ResponseLines.Skip(1))
            {
                if (line.StartsWith("*") || string.IsNullOrWhiteSpace(line)) 
                    continue;
                if (string.Equals(line, ")"))
                    break;
                if (line.StartsWith(" "))
                {
                    kvp[lastKey] += line;
                }
                else
                {
                    var indexOf = line.IndexOf(":");
                    var key = line.Substring(0, indexOf);
                    kvp[key] = line.Substring(indexOf + 2);
                    lastKey = key;
                }
            }
            if(kvp.ContainsKey("Date"))
            {
                //BUG: This needs to be abstracted, corrected and tested. Dates like Mon, 1 Jan 2000 12:00:00 +12:00 DST break due to the 'DST' suffix.
                //date = DateTimeOffset.ParseExact(kvp["Date"], "ddd, d MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture);
                date = DateTimeOffset.ParseExact(kvp["Date"],
                    new[]
                        {
                          "ddd, d MMM yyyy HH:mm:ss zzz", 
                          "d MMM yyyy HH:mm:ss zzz"  
                        }, 
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None);
                isDateSet = true; 
            }
            if (kvp.ContainsKey("Subject"))
            {
                subject = kvp["Subject"];
                isSubjectSet = true;
            }
            if (kvp.ContainsKey("From"))
            {
                //HACK: Need to have this provided
                if(kvp["From"].ToLower().Contains("lee.ryan.campbell@gmail.com"))
                {
                    direction = MessageDirection.Outbound;
                }
            } 
           
            /*
[<--]Date: Thu, 27 Sep 2012 09:13:40 +0100
[<--]Message-ID: <CAPLQusCjGquLW-shZiFkSEWnDZkWO1j+Kc6_aGvtpJybqiRdFQ@mail.gmail.com>
[<--]Subject: Erynne's details
[<--]From: Lee Campbell <lee.ryan.campbell@gmail.com>
[<--]To: Marcus Whitworth <hello@marcuswhitworth.com>

             */
            if (isDateSet && isSubjectSet)
            {
                return new GmailEmail(date, direction, subject, null);
            }

            return null;
        }
    }
}