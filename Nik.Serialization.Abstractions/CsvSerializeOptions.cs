namespace Nik.Serialization.Abstractions;

public sealed class CsvSerializeOptions
{
    public string Separator { get; set; } = ";";
    public bool HasHeader { get; set; } = true;
    public Encoding Encoding { get; set; } = Encoding.Latin1;
}