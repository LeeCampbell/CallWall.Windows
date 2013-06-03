using System.IO;
using System.IO.IsolatedStorage;

namespace CallWall
{
    public sealed class LocalStoragePersistence : ILocalStoragePersistence
    {
        public string Read(string key)
        {
            string content = null;
            var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            using (var srReader = new StreamReader(new IsolatedStorageFileStream(key, FileMode.OpenOrCreate, isolatedStorage)))
            {
                content = srReader.ReadToEnd();
                srReader.Close();
            }
            return content;
        }

        public void Write(string key, string content)
        {
            var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();

            //create a stream writer object to write content in the location
            using (var srWriter = new StreamWriter(new IsolatedStorageFileStream(key, FileMode.Create, isolatedStorage)))
            {
                srWriter.Write(content);
                srWriter.Flush();
                srWriter.Close();
            }
        }
    }
}