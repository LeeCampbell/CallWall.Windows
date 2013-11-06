using System.IO;
using System.Net.Security;

namespace CallWall.Windows.Google.Providers.Gmail.Imap
{
    public interface IIMapOperation
    {
        bool Execute(string prefix, SslStream sendStream, StreamReader receiveStream);
    }
}