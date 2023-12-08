namespace Nik.Serialization;

public sealed class CsvSerializer : ICsvSerializer
{
    private static readonly string NullValue = new Guid().ToString("N");

    public string Serialize<T>(IEnumerable<T> values, CsvSerializeOptions options) where T : class, new()
    {
        var linkedProperties = GetLinkedProperties<T>().ToList();
        var result = string.Empty;

        if (options.HasHeader)
        {
            string header = string.Join(options.Separator, linkedProperties.OrderBy(property => property.CsvIndex).Select(property => property.CsvName));
            GenerateIndexes(linkedProperties, header, options);
            result = header + Environment.NewLine;
        }

        foreach (var value in values)
        {
            result += string.Join(options.Separator, GetValueList(linkedProperties, value)).Replace(NullValue, string.Empty) + Environment.NewLine;
        }

        return result.Trim();
    }

    private static IEnumerable<object> GetValueList<T>(List<PropertyFieldLink> linkedProperties, T value)
    {
        foreach (var linkedProperty in linkedProperties.OrderBy(property => property.CsvIndex))
        {
            object? obj = linkedProperty.Property!.GetValue(value);
            yield return obj ?? NullValue;
        }
    }

    public IEnumerable<T> Deserialize<T>(IEnumerable<string> csvLines, CsvSerializeOptions options, bool checkFieldsCount) where T : class, new()
    {
        var lines = GetValidLines(csvLines);
        if (!lines.Any())
        {
            yield break;
        }

        var hasRedundantSeparator = false;
        var linkedProperties = GetLinkedProperties<T>().ToList();

        if (options.HasHeader)
        {
            var header = lines[0];
            lines.RemoveAt(0);
            hasRedundantSeparator = header[^options.Separator.Length..] == options.Separator;
            if (hasRedundantSeparator)
            {
                header = header[..^options.Separator.Length];
            }

            GenerateIndexes(linkedProperties, header, options);
        }

        foreach (var line in lines)
        {
            var value = GetValue<T>(line, linkedProperties, options, checkFieldsCount, hasRedundantSeparator);
            yield return value;
        }
    }

    private static void GenerateIndexes(List<PropertyFieldLink> linkedProperties, string header, CsvSerializeOptions options)
    {
        var headers = header.Split(options.Separator);
        var headerItems = Enumerable.Range(0, headers.Length).ToDictionary(i => headers[i], i => i);

        foreach (var property in linkedProperties)
        {
            property.CsvIndex = headerItems.FirstOrDefault(header => header.Key.Equals(property.CsvName, StringComparison.CurrentCultureIgnoreCase)).Value;
        }
    }

    private static List<string> GetValidLines(IEnumerable<string> csvLines)
    {
        var lines = (csvLines as List<string>) ?? csvLines.ToList();
        lines.ForEach(line => line = line.Trim());

        lines.RemoveAll(line => string.IsNullOrWhiteSpace(line));

        return lines;
    }

    private static IEnumerable<PropertyFieldLink> GetLinkedProperties<T>() where T : class, new()
    {
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var field = property.GetCustomAttributes(typeof(CsvFieldAttribute), false).FirstOrDefault();
            if (field != null)
            {
                yield return new PropertyFieldLink
                {
                    Property = property,
                    CsvName = ((CsvFieldAttribute)field).Name,
                    CsvIndex = ((CsvFieldAttribute)field).Index
                };
            }
        }
    }

    private static T GetValue<T>(string line, List<PropertyFieldLink> linkedProperties, CsvSerializeOptions options, bool checkFieldsCount, bool hasRedundantSeparator) where T : class, new()
    {
        if (hasRedundantSeparator)
        {
            line = line[..^options.Separator.Length];
        }
        var csvFields = line.Split(options.Separator);

        if (checkFieldsCount && csvFields.Length > linkedProperties.Count)
        {
            throw new Exception("Number of the value fields is more than expected.");
        }

        var data = new T();
        foreach (var linkedProperty in linkedProperties)
        {
            object? value;
            string stringValue = csvFields[linkedProperty.CsvIndex];
            if (string.IsNullOrEmpty(stringValue))
            {
                value = default;
            }
            else if (linkedProperty.GetType().IsEnum)
            {
                value = Enum.ToObject(linkedProperty.GetType(), Convert.ToInt32(stringValue));
            }
            else
            {
                value = Convert.ChangeType(stringValue, linkedProperty.Property!.PropertyType, CultureInfo.InvariantCulture);
            }

            linkedProperty.Property!.SetValue(data, value);
        }

        return data;
    }

    private sealed class PropertyFieldLink
    {
        public PropertyInfo? Property { get; set; }
        public string CsvName { get; set; } = string.Empty;
        public int CsvIndex { get; set; }
    }
}