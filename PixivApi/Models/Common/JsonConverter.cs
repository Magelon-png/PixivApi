using System.Globalization;
using System.Text.Json;
using Scighost.PixivApi.Models.Illust;
using Scighost.PixivApi.Models.Novel;
using Scighost.PixivApi.SerializerContexts;

namespace Scighost.PixivApi.Models.Common;

internal sealed class DictionaryKeyToListJsonConverter<T> : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(List<int>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(List<int>))
        {
            return new DictionaryKeyToListJsonConverterInt32();
        }
        

        throw new NotSupportedException($"Type {typeToConvert} is not supported by {nameof(DictionaryKeyToListJsonConverter<T>)}.");
    }
}


internal sealed class DictionaryKeyToListJsonConverterInt32 : JsonConverter<List<int>>
{
    public override List<int>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<List<int>>(ref reader, PixivJsonSerializerContext.Default.ListInt32);
        }
        else
        {
            var dic = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.DictionaryInt32Object);
            return dic?.Keys.ToList();
        }
    }

    public override void Write(Utf8JsonWriter writer, List<int> value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.ListInt32));
    }
}

internal sealed class DictionaryValueToListJsonConverter<T> : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(List<NovelProfile>) ||
               typeToConvert == typeof(List<IllustProfile>) ||
               typeToConvert == typeof(List<string>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(List<NovelProfile>))
        {
            return new DictionaryValueToListJsonConverterNovelProfile();
        }
        else if (typeToConvert == typeof(List<IllustProfile>))
        {
            return new DictionaryValueToListJsonConverterIllustProfile();
        }
        else if (typeToConvert == typeof(List<string>))
        {
            return new DictionaryValueToListJsonConverterString();
        }

        throw new NotSupportedException($"Type {typeToConvert} is not supported by {nameof(DictionaryValueToListJsonConverter<T>)}.");
    }
}

internal sealed class DictionaryValueToListJsonConverterNovelProfile : JsonConverter<List<NovelProfile>>
{
    public override List<NovelProfile>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<List<NovelProfile>>(ref reader, PixivJsonSerializerContext.Default.ListNovelProfile);
        }
        else
        {
            var dic = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.DictionaryInt32NovelProfile);
            return dic?.Values.ToList();
        }
    }

    public override void Write(Utf8JsonWriter writer, List<NovelProfile> value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.ListNovelProfile));
    }
}

internal sealed class DictionaryValueToListJsonConverterIllustProfile : JsonConverter<List<IllustProfile>>
{
    public override List<IllustProfile>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<List<IllustProfile>>(ref reader, PixivJsonSerializerContext.Default.ListIllustProfile);
        }
        else
        {
            var dic = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.DictionaryInt32IllustProfile);
            return dic?.Values.ToList();
        }
    }

    public override void Write(Utf8JsonWriter writer, List<IllustProfile> value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.ListIllustProfile));
    }
}

internal sealed class DictionaryValueToListJsonConverterString : JsonConverter<List<string>>
{
    public override List<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<List<string>>(ref reader, PixivJsonSerializerContext.Default.ListString);
        }
        else
        {
            var dic = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.DictionaryInt32String);
            return dic?.Values.ToList();
        }
    }

    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.ListString));
    }
}


internal sealed class ListLongAsDictionaryIndexJsonConverter : JsonConverter<List<long>>
{
    public override List<long>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dic = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.DictionaryInt32String);
        if (dic is null)
            return null;

        return dic.OrderBy(kv => kv.Key)
                  .Select(kv => long.Parse(kv.Value, CultureInfo.InvariantCulture))
                  .ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<long> value, JsonSerializerOptions options)
    {
        var dic = value.Select((id, index) => (index, id.ToString(CultureInfo.InvariantCulture)))
                       .ToDictionary(t => t.index, t => t.Item2);
        writer.WriteRawValue(JsonSerializer.Serialize(dic, PixivJsonSerializerContext.Default.DictionaryInt32String));
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

internal sealed class ChangeMangaSeriesWatchListNotificationResponseJsonConverter : JsonConverter<ChangeMangaSeriesWatchListNotificationResponse>
{
    public override ChangeMangaSeriesWatchListNotificationResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) { }
            return new ChangeMangaSeriesWatchListNotificationResponse(null);
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            bool? notifiable = null;
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "notifiable")
                {
                    reader.Read();
                    notifiable = reader.GetBoolean();
                }
                else
                {
                    reader.Skip();
                }
            }
            return new ChangeMangaSeriesWatchListNotificationResponse(notifiable);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, ChangeMangaSeriesWatchListNotificationResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.Notifiable.HasValue)
        {
            writer.WriteBoolean("notifiable", value.Notifiable.Value);
        }
        writer.WriteEndObject();
    }
}

internal sealed class EmptyArrayAsDictionaryJsonConverter<T> : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Dictionary<string, PlanTranslationDescription>) ||
               typeToConvert == typeof(Dictionary<string, PlanTranslationTitle>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(Dictionary<string, PlanTranslationDescription>))
        {
            return new EmptyArrayAsDictionaryJsonConverterPlanTranslationDescription();
        }
        else if (typeToConvert == typeof(Dictionary<string, PlanTranslationTitle>))
        {
            return new EmptyArrayAsDictionaryJsonConverterPlanTranslationTitle();
        }

        throw new NotSupportedException($"Type {typeToConvert} is not supported by {nameof(EmptyArrayAsDictionaryJsonConverter<T>)}.");
    }
}

internal sealed class EmptyArrayAsDictionaryJsonConverterPlanTranslationDescription : JsonConverter<Dictionary<string, PlanTranslationDescription>> 
{
    public override Dictionary<string, PlanTranslationDescription>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // If it's an array (even if empty), return an empty dictionary
            // Read through the array without using Skip() since it may be partial JSON
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                // Just iterate through the array elements
            }
            return new Dictionary<string, PlanTranslationDescription>();
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Normal dictionary deserialization
            return JsonSerializer.Deserialize<Dictionary<string, PlanTranslationDescription>>(ref reader, PixivJsonSerializerContext.Default.DictionaryStringPlanTranslationDescription);
        }
        
        return null;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, PlanTranslationDescription> value, JsonSerializerOptions options)
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
            writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.DictionaryStringPlanTranslationDescription));
        }
    }
}

internal sealed class EmptyArrayAsDictionaryJsonConverterPlanTranslationTitle : JsonConverter<Dictionary<string, PlanTranslationTitle>> 
{
    public override Dictionary<string, PlanTranslationTitle>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // If it's an array (even if empty), return an empty dictionary
            // Read through the array without using Skip() since it may be partial JSON
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                // Just iterate through the array elements
            }
            return new Dictionary<string, PlanTranslationTitle>();
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Normal dictionary deserialization
            return JsonSerializer.Deserialize<Dictionary<string, PlanTranslationTitle>>(ref reader, PixivJsonSerializerContext.Default.DictionaryStringPlanTranslationTitle);
        }
        
        return null;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, PlanTranslationTitle> value, JsonSerializerOptions options)
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
            writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.DictionaryStringPlanTranslationTitle));
        }
    }
}
