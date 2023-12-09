namespace Nik.Serialization.UnitTests;

public sealed class CsvSerializerPartialTests
{
    [Fact]
    public void TestEnumCsvDeserialize()
    {
        List<string> testCsv = ["code;name;grade", "C1;arash;20", "C2;borzoo;19"];

        CsvSerializer serializer = new();
        var data = serializer.Deserialize<Data>(testCsv, new() { HasHeader = true, Separator = ";" }, false).ToList();
        data.Should().HaveCount(2);
        data[0].Code.Should().Be("C1");
        data[1].Name.Should().Be("borzoo");
        data[1].Address.Should().BeNull();
    }

    public class Data
    {
        [CsvField(Name = "code")]
        public string Code { get; set; }

        [CsvField(Name = "name")]
        public string Name { get; set; }

        public string Address { get; set; }
    }
}