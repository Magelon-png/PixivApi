using System.Text.Json;

namespace Scighost.PixivApi.Models.Common;


internal sealed class DictionaryKeyToListJsonConverter<T> : JsonConverter<List<T>> where T : notnull
{
    public override List<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<List<T>>(ref reader, options);
        }
        else
        {
            var dic = JsonSerializer.Deserialize<Dictionary<T, object>>(ref reader, options);
            return dic?.Keys.ToList();
        }
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value, options));
    }
}


internal sealed class DictionaryValueToListJsonConverter<TKey, TValue> : JsonConverter<List<TValue>> where TKey : notnull
{
    public override List<TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<List<TValue>>(ref reader, options);
        }
        else
        {
            var dic = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options);
            return dic?.Values.ToList();
        }
    }

    public override void Write(Utf8JsonWriter writer, List<TValue> value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value, options));
    }
}


internal sealed class BoolToNumberJsonConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var num = reader.GetInt32();
        return num != 0;
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value ? 1 : 0);
    }
}


internal sealed class EmptyArrayAsDictionaryJsonConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>> 
    where TKey : notnull
{
    public override Dictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // If it's an array (even if empty), return an empty dictionary
            // Read through the array without using Skip() since it may be partial JSON
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                // Just iterate through the array elements
            }
            return new Dictionary<TKey, TValue>();
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Normal dictionary deserialization
            return JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options);
        }
        
        return null;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
    {
        if (value.Count == 0)
        {
            // Write empty array instead of empty object
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else
        {
            // Normal dictionary serialization
            writer.WriteRawValue(JsonSerializer.Serialize(value, options));
        }
    }
}
