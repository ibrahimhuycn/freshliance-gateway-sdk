using System.Text.Json;
using System.Text.Json.Serialization;

namespace FreshlianceGateway.Sdk.Core;

internal class BizContentJsonConverter : JsonConverter<object>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize(ref reader, typeToConvert, options);

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        if (value is string str)
        {
            writer.WriteStringValue(str);
        }
        else
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
