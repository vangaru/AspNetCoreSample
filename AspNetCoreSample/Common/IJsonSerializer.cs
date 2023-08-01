using Newtonsoft.Json.Serialization;

namespace AspNetCoreSample.Common;

public interface IJsonSerializer
{
    T Deserialize<T>(string json, IContractResolver contractResolver = null);

    string Serialize(object obj, IContractResolver contractResolver = null);

    byte[] SerializeToUtf8Bytes(object obj);
}