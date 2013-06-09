using System;

namespace CallWall.Google.Providers.Gmail.Imap
{
    public interface IImapDateTranslator
    {
        DateTimeOffset Translate(string imapDate);
    }
}