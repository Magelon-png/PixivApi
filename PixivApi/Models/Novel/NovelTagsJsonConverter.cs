using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scighost.PixivApi.Models.Novel;

/// <summary>
/// Simple tag structure for V2 API responses
/// </summary>
internal sealed record TagV2(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("count")]
    int? Count,
    [property: JsonPropertyName("is_registered")]
    bool? IsRegistered
);

/// <summary>
/// JSON converter for tags property in NovelProfile.
/// Converts from array of Tag objects to List&lt;string&gt; by extracting tag names.
/// </summary>
internal sealed class NovelTagsJsonConverter : JsonConverter<List<string>>
{
    public override List<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var tags = JsonSerializer.Deserialize<List<TagV2>>(ref reader, options);
        return tags?.Select(t => t.Name).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
    {
        // For writing, convert back to Tag objects with just name
        var tags = value.Select(name => new TagV2(Name: name, Count: null, IsRegistered: null)).ToList();
        JsonSerializer.Serialize(writer, tags, options);
    }
}
