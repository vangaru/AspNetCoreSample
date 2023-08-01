using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AspNetCoreSample.Common;

public sealed class JsonSerializer : IJsonSerializer
{
    public T Deserialize<T>(string json, IContractResolver contractResolver = null)
    {
        return JsonConvert.DeserializeObject<T>(json, GetJsonSerializerSettings(contractResolver));
    }

    public string Serialize(object obj, IContractResolver contractResolver = null)
    {
        return JsonConvert.SerializeObject(obj, GetJsonSerializerSettings(contractResolver));
    }

    public byte[] SerializeToUtf8Bytes(object obj)
    {
        return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(obj);
    }


    private static JsonSerializerSettings GetJsonSerializerSettings(IContractResolver contractResolver = null) => new()
    {
        ContractResolver =  contractResolver ?? new CamelCasePropertyNamesContractResolver(),
        Converters = { new StringEnumConverter() },
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
    };
}