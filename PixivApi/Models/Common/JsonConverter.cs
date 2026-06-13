using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Scighost.PixivApi.Models.Collection;
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
               typeToConvert == typeof(Dictionary<string, PlanTranslationTitle>)
               || typeToConvert == typeof(Dictionary<BigInteger, JsonNode?>);
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
        else if(typeToConvert == typeof(Dictionary<BigInteger, JsonNode?>))
        {
            return new EmptyArrayAsDictionaryJsonConverterJsonNodeNullable();
        }

        throw new NotSupportedException($"Type {typeToConvert} is not supported by {nameof(EmptyArrayAsDictionaryJsonConverter<T>)}.");
    }
}


internal sealed class EmptyArrayAsDictionaryJsonConverterJsonNodeNullable : JsonConverter<Dictionary<BigInteger, JsonNode>>
{

    public override Dictionary<BigInteger, JsonNode>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) { }
            return new Dictionary<BigInteger, JsonNode>();
        }

        if (reader.TokenType != JsonTokenType.StartObject)
            return null;

        var result = new Dictionary<BigInteger, JsonNode>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            var key = BigInteger.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
            reader.Read();
            var value = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.JsonNode);
            result[key] = value!;
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<BigInteger, JsonNode> value, JsonSerializerOptions options)
    {
        if (value.Count == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key.ToString(CultureInfo.InvariantCulture));
                writer.WriteRawValue(JsonSerializer.Serialize(kvp.Value, PixivJsonSerializerContext.Default.JsonNode));
            }
            writer.WriteEndObject();
        }
    }
    
}

internal sealed class EmptyArrayAsDictionaryJsonConverterPlanTranslationDescription : JsonConverter<Dictionary<string, PlanTranslationDescription>>
{
    public override Dictionary<string, PlanTranslationDescription>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) { }
            return new Dictionary<string, PlanTranslationDescription>();
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<Dictionary<string, PlanTranslationDescription>>(ref reader, PixivJsonSerializerContext.Default.DictionaryStringPlanTranslationDescription);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, PlanTranslationDescription> value, JsonSerializerOptions options)
    {
        if (value.Count == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else
        {
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
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) { }
            return new Dictionary<string, PlanTranslationTitle>();
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<Dictionary<string, PlanTranslationTitle>>(ref reader, PixivJsonSerializerContext.Default.DictionaryStringPlanTranslationTitle);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, PlanTranslationTitle> value, JsonSerializerOptions options)
    {
        if (value.Count == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else
        {
            writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.DictionaryStringPlanTranslationTitle));
        }
    }
}

internal sealed class EmptyArrayAsEmptyDictionaryConverterCollectionTagTranslation : JsonConverter<Dictionary<string, CollectionTagTranslation>>
{
    public override Dictionary<string, CollectionTagTranslation>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) { }
            return new Dictionary<string, CollectionTagTranslation>();
        }
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.DictionaryStringCollectionTagTranslation);
        }
        return null;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, CollectionTagTranslation> value, JsonSerializerOptions options)
    {
        if (value.Count == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else
        {
            writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.DictionaryStringCollectionTagTranslation));
        }
    }
}

internal sealed class BigIntegerConverter : JsonConverter<BigInteger>
{
    public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return BigInteger.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
        }
        if (reader.TokenType == JsonTokenType.Number)
        {
            return new BigInteger(reader.GetInt64());
        }
        return BigInteger.Zero;
    }

    public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}

internal sealed class BigIntegerDictionaryConverterCollectionProfile : JsonConverter<Dictionary<BigInteger, CollectionProfile>>
{
    public override Dictionary<BigInteger, CollectionProfile>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            return null;

        var result = new Dictionary<BigInteger, CollectionProfile>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            var key = BigInteger.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
            reader.Read();
            var value = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.CollectionProfile);
            if (value is not null)
                result[key] = value;
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<BigInteger, CollectionProfile> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key.ToString(CultureInfo.InvariantCulture));
            writer.WriteRawValue(JsonSerializer.Serialize(kvp.Value, PixivJsonSerializerContext.Default.CollectionProfile));
        }
        writer.WriteEndObject();
    }
}

internal sealed class BigIntegerListConverter : JsonConverter<List<BigInteger>>
{
    public override List<BigInteger>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            return null;

        var list = new List<BigInteger>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType == JsonTokenType.String)
                list.Add(BigInteger.Parse(reader.GetString()!, CultureInfo.InvariantCulture));
            else if (reader.TokenType == JsonTokenType.Number)
                list.Add(new BigInteger(reader.GetInt64()));
        }
        return list;
    }

    public override void Write(Utf8JsonWriter writer, List<BigInteger> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
            writer.WriteStringValue(item.ToString(CultureInfo.InvariantCulture));
        writer.WriteEndArray();
    }
}

internal sealed class BookmarkPeriodDateConverter : JsonConverter<DateOnly> 
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            return default;

        var date = reader.GetString();
        
        if(date is null)
            return default;

        var yearAndMonth = date.Split('-');
        
        if(yearAndMonth.Length != 2)
            return default;
        if(!int.TryParse(yearAndMonth[0], out var year))
            return default;
        if(!int.TryParse(yearAndMonth[1], out var month))
            return default;
        
        return new DateOnly(year, month, 1);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM", CultureInfo.InvariantCulture));
    }
}

internal sealed class BookmarkTagsConverter : JsonConverter<Dictionary<long, string[]>>
{
    public override Dictionary<long, string[]>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) { }
            return new Dictionary<long, string[]>();
        }

        if (reader.TokenType != JsonTokenType.StartObject)
            return null;

        var result = new Dictionary<long, string[]>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            var key = long.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
            reader.Read();
            var value = JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.ListString);
            result[key] = value?.ToArray() ?? [];
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<long, string[]> value, JsonSerializerOptions options)
    {
        if (value.Count == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key.ToString(CultureInfo.InvariantCulture));
                writer.WriteStartArray();
                foreach (var tag in kvp.Value)
                    writer.WriteStringValue(tag);
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }
    }
}
