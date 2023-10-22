namespace Nik.Serialization;

public sealed class XMLSerializer : IXMLSerializer
{
    public string Serialize<T>(T obj) where T : class, new()
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        serializer.Serialize(writer, obj);
        writer.Close();
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}