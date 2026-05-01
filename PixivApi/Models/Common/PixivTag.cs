using System.Text.Json;
using Scighost.PixivApi.SerializerContexts;

namespace Scighost.PixivApi.Models.Common;


/// <summary>
/// Original tag information of a work. The corresponding JSON structure is somewhat complex; to reduce the number of entity classes, different properties of this class are valid in different situations.
/// <para />
/// Users other than the author can also add tags to the work. After editing a tag, a notification will be sent to the author. The author can accept and lock the tag to make it non-editable, or lock all tags of the work.
/// </summary>
internal sealed record PixivTagInternal(
    [property: JsonPropertyName("authorId"),
    JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    int AuthorId,
    [property: JsonPropertyName("isLocked")]
    bool IsLocked,
    [property: JsonPropertyName("tags")]
    List<PixivTagInternal> Tags,
    [property: JsonPropertyName("writable")]
    bool Writable,
    [property: JsonPropertyName("tag")]
    string Tag,
    [property: JsonPropertyName("locked")]
    bool Locked,
    [property: JsonPropertyName("deletable")]
    bool Deletable,
    [property: JsonPropertyName("userId")]
    string UserId,
    [property: JsonPropertyName("userName")]
    string UserName,
    [property: JsonPropertyName("translation")]
    TagTranslationInternal Translation
);


/// <summary>
/// Tag translation
/// </summary>
internal sealed record TagTranslationInternal(
    [property: JsonPropertyName("en")]
    string Translation
);


/// <summary>
/// Work tag
/// </summary>
public record PixivTag(
    [property: JsonPropertyName("tag")]
    string Tag,
    [property: JsonPropertyName("translation")]
    string? Translation,
    [property: JsonPropertyName("locked")]
    bool Locked,
    [property: JsonPropertyName("writable")]
    bool Writable,
    [property: JsonPropertyName("deletable")]
    bool Deletable,
    [property: JsonPropertyName("userId")]
    string UserId,
    [property: JsonPropertyName("userName")]
    string UserName
);



internal sealed class PixivTagJsonConverter : JsonConverter<List<PixivTag>>
{
    public override List<PixivTag>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize(ref reader, PixivJsonSerializerContext.Default.ListPixivTag);
        }
        else
        {
            var tag = JsonSerializer.Deserialize<PixivTagInternal>(ref reader, PixivJsonSerializerContext.Default.PixivTagInternal);
            return tag?.Tags.Select(t => new PixivTag(
                Deletable: t.Deletable,
                Locked: t.Locked,
                Tag: t.Tag,
                Translation: t.Translation?.Translation,
                UserId: t.UserId,
                UserName: t.UserName,
                Writable: t.Writable
            )).ToList();
        }
    }

    public override void Write(Utf8JsonWriter writer, List<PixivTag> value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(JsonSerializer.Serialize(value, PixivJsonSerializerContext.Default.ListPixivTag));
    }
}