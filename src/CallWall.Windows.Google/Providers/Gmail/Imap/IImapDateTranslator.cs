using System;

namespace CallWall.Windows.Google.Providers.Gmail.Imap
{
    public interface IImapDateTranslator
    {
        DateTimeOffset Translate(string imapDate);
    }
}