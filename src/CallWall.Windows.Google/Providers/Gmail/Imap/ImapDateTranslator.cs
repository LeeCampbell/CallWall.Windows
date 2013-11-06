using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CallWall.Windows.Google.Providers.Gmail.Imap
{
    public sealed class ImapDateTranslator : IImapDateTranslator
    {
        private readonly Regex _regex = new Regex(@"\d{1,2} \w{3} \d{4} \d{2}:\d{2}:\d{2} (\-|\+)\d{2}:?\d{2}", RegexOptions.Compiled);
        
        public DateTimeOffset Translate(string imapDate)
        {
            var regexMatch = _regex.Match(imapDate);
            if (regexMatch.Success)
            {
                //return DateTimeOffset.ParseExact(regexMatch.Value, "d MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.None);
                //return DateTimeOffset.Parse(regexMatch.Value);
                
                DateTimeOffset output;
                if (DateTimeOffset.TryParse(regexMatch.Value, out output))
                    return output;
            }
            throw new InvalidDataException(string.Format("The date is in an unexpected format : {0}", imapDate));
        }
    }
}