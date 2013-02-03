using CallWall.Contract.Contact;

namespace CallWall.Google.Providers
{
    public interface IGoogleContactProfileTranslator
    {
        IContactProfile Translate(string response, string accessToken);
    }
}