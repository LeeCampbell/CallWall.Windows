namespace CallWall.Google.Providers
{
    public interface IGoogleContactProfileTranslator
    {
        IGoogleContactProfile Translate(string response, string accessToken);
        IGoogleContactProfile AddTags(IGoogleContactProfile contactProfile, string response);
    }
}