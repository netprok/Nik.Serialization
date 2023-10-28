namespace Nik.Serialization.Models;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class CsvFieldAttribute : Attribute
{
    public string Name { get; set; } = string.Empty;
    public int Index { get; set; }
}