namespace CallWall
{
    public interface IJsonSerializer
    {
        string Serialize<TSource>(TSource source);
        TSource Deserialize<TSource>(string payload);
    }
}