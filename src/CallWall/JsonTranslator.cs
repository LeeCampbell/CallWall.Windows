using Newtonsoft.Json;

namespace CallWall
{
    public sealed class JsonTranslator<TSource> : ITwoWayTranslator<TSource, string>
    {
        public string Translate(TSource source)
        {
            return JsonConvert.SerializeObject(source, Formatting.Indented);
        }

        public TSource TranslateBack(string target)
        {
            return JsonConvert.DeserializeObject<TSource>(target);
        }
    }
}