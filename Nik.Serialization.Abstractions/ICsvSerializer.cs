namespace Nik.Serialization.Abstractions;

public interface ICsvSerializer
{
    string Serialize<T>(IEnumerable<T> values, CsvSerializeOptions options) where T : class, new();

    IEnumerable<T> Deserialize<T>(IEnumerable<string> csvLines, CsvSerializeOptions options, bool checkFieldsCount) where T : class, new();
}
