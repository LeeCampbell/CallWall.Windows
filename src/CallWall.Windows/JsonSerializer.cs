using Newtonsoft.Json;

namespace CallWall
{
    public sealed class JsonSerializer : IJsonSerializer
    {
        public string Serialize<TSource>(TSource source)
        {
            return JsonConvert.SerializeObject(source, Formatting.Indented);
        }

        public TSource Deserialize<TSource>(string payload)
        {
            return JsonConvert.DeserializeObject<TSource>(payload);
        }
    }
}