using System.Collections.Generic;

namespace CallWall
{
    public sealed class PersonalizationSettings : IPersonalizationSettings
    {
        private readonly ILocalStoragePersistence _localStorage;
        private readonly IJsonSerializer _translator;
        private readonly object _gate = new object();
        private const string FileKey = "LocalStoreSettings";
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>();
        private bool _loaded;

        public PersonalizationSettings(ILocalStoragePersistence localStorage, IJsonSerializer translator)
        {
            _localStorage = localStorage;
            _translator = translator;
        }

        public string Get(string key)
        {
            LoadIfRequired();
            string value;
            lock (_gate)
            {
                _data.TryGetValue(key, out value);
            }
            return value;
        }

        public void Put(string key, string value)
        {
            LoadIfRequired();
            lock (_gate)
            {
                _data[key] = value;
                SaveData();
            }
        }

        public void Remove(string key)
        {
            LoadIfRequired();

            lock (_gate)
            {
                _data.Remove(key);
                SaveData();
            }
        }


        private void LoadIfRequired()
        {
            if (_loaded)
                return;
            lock (_gate)
            {
                if (_loaded)
                    return;

                var payload = _localStorage.Read(FileKey);
                var data = _translator.Deserialize<Dictionary<string, string>>(payload) ?? new Dictionary<string, string>();

                _data.Clear();
                foreach (var key in data.Keys)
                {
                    _data[key] = data[key];
                }

                _loaded = true;
            }
        }

        private void SaveData()
        {
            var payload = _translator.Serialize(_data);
            _localStorage.Write(FileKey, payload);
        }
    }
}
