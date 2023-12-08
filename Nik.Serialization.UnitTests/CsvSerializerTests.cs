using FluentAssertions;

using Nik.Serialization.Models;

namespace Nik.Serialization.UnitTests;

public sealed class CsvSerializerTests
{
    [Fact]
    public void TestEnumCsvDeserialize()
    {
        List<string> testCsv = ["kind;name", "Created;arash", "2;borzoo"];

        CsvSerializer serializer = new();
        var data = serializer.Deserialize<Data>(testCsv, new() { HasHeader = true, Separator = ";" }, true).ToList();
        data.Should().HaveCount(2);
        data[0].Kind.Should().Be(EKind.Created);
        data[1].Kind.Should().Be(EKind.Saved);
    }

    public class Data
    {
        [CsvField(Name = "kind")]
        public EKind Kind { get; set; }

        [CsvField(Name = "name")]
        public string Name { get; set; }
    }

    public enum EKind
    {
        Created = 1,
        Saved = 2
    }
}