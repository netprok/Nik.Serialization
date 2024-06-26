﻿namespace Nik.Serialization;

/// <summary>
/// Does the json tasks using Newtonsoft.Json.
/// </summary>
public sealed class NewtonsoftJsonSerializer : IJsonSerializer
{
    public T? Deserialize<T>(string json) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(
            json,
            new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });
    }

    public string Serialize(object value, bool forceCamelCase) =>
        forceCamelCase ?
            Newtonsoft.Json.JsonConvert.SerializeObject(
                value,
                new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }
            ) :
            Newtonsoft.Json.JsonConvert.SerializeObject(value);
}