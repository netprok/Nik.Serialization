namespace Nik.Serialization.Abstractions;

public interface IXMLSerializer
{
    string Serialize<T>(T obj) where T : class, new();
}
