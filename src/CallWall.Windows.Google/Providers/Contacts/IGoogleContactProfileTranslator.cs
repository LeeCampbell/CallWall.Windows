namespace CallWall.Windows.Google.Providers.Contacts
{
    public interface IGoogleContactProfileTranslator
    {
        IGoogleContactProfile Translate(string response, string accessToken);
        IGoogleContactProfile AddTags(IGoogleContactProfile contactProfile, string response);
        GoogleUser GetUser(string response);
    }
}