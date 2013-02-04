using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace CallWall.Services
{
    public sealed class PersonalizationSettings : IPersonalizationSettings
    {
        private readonly object _gate = new object();
        private const string FileKey = "LocalStoreSettings";
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>();

        public PersonalizationSettings()
        {
            var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            using (var srReader = new StreamReader(new IsolatedStorageFileStream(FileKey, FileMode.OpenOrCreate, isolatedStorage)))
            {
                string payload = srReader.ReadToEnd();
                _data = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload)
                    ?? new Dictionary<string, string>();
                _data = new Dictionary<string, string>();
                srReader.Close();
            }
        }

        public string Get(string key)
        {
            string value;
            lock (_gate)
            {
                _data.TryGetValue(key, out value);
            }
            return value;
        }

        public void Put(string key, string value)
        {
            lock (_gate)
            {
                _data[key] = value;
                SaveData();
            }
        }

        public void Remove(string key)
        {
            lock (_gate)
            {
                _data.Remove(key);
                SaveData();
            }
        }

        private void SaveData()
        {
            var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();

            //create a stream writer object to write content in the location
            using (var srWriter = new StreamWriter(new IsolatedStorageFileStream(FileKey, FileMode.Create, isolatedStorage)))
            {
                var payload = JsonConvert.SerializeObject(_data, Formatting.Indented);
                srWriter.Write(payload);
                srWriter.Flush();
                srWriter.Close();
            }
        }
   
    }
}
