using Recruiva.Web.ValueObjects;

using System.Text.Json.Serialization;

namespace Recruiva.Web.Converters;

public class NameConverter : JsonConverter<Name>
{
    public override Name Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Name.Create(value!);
    }

    public override void Write(Utf8JsonWriter writer, Name value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}