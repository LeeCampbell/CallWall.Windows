using System.IO;
using System.IO.IsolatedStorage;

namespace CallWall
{
    public sealed class LocalStoragePersistence : ILocalStoragePersistence
    {
        public string Read(string key)
        {
            string content = null;
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            using (var srReader = new StreamReader(new IsolatedStorageFileStream(key, FileMode.OpenOrCreate, isolatedStorage)))
            {
                content = srReader.ReadToEnd();
            }
            return content;
        }

        public void Write(string key, string content)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            using (var srWriter = new StreamWriter(new IsolatedStorageFileStream(key, FileMode.Create, isolatedStorage)))
            {
                srWriter.Write(content);
                srWriter.Flush();
            }
        }

        public void Reset()
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                isolatedStorage.Remove();
            }
        }
    }
}