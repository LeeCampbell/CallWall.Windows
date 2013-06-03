namespace CallWall
{
    public interface ILocalStoragePersistence
    {
        string Read(string key);
        void Write(string key, string content);
    }
}