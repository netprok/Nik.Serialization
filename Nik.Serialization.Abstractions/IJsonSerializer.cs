namespace Nik.Serialization.Abstractions;

/// <summary>
/// Handles the tasks related to json files.
/// </summary>
public interface IJsonSerializer
{
    /// <summary>
    /// Desrializes a json text as the given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    T Deserialize<T>(string json) where T : class, new();

    /// <summary>
    /// Serializes an object to a json string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string Serialize(object value, bool forceCamelCase);
}
