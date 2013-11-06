namespace CallWall.Windows
{
    public interface IPersonalizationSettings
    {
        string Get(string key);
        void Remove(string key);
        void Put(string key, string value);
        void ClearAll();
    }

    public static class PersonalizationSettingsExtensions
    {
        public static bool GetAsBool(this IPersonalizationSettings settings, string key, bool fallbackValue)
        {
            var value = settings.Get(key);
            return value == null 
                ? fallbackValue 
                : bool.Parse(value);
        }

        public static void SetAsBool(this IPersonalizationSettings settings, string key, bool value)
        {
            settings.Put(key, value.ToString());
        }
    }
}