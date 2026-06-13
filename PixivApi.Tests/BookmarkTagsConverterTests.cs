using System.Text.Json;
using Scighost.PixivApi.Models.Common;

namespace PixivApi.Tests;

[TestClass]
public sealed class BookmarkTagsConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new BookmarkTagsConverter() }
    };

    [TestMethod]
    public void Read_EmptyArray_ReturnsEmptyDictionary()
    {
        var result = JsonSerializer.Deserialize<Dictionary<long, string[]>>("[]", Options);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void Read_ObjectWithEntries_ReturnsPopulatedDictionary()
    {
        var json = /*lang=json*/ """{"123": ["tag1", "tag2"], "456": ["tag3"]}""";
        var result = JsonSerializer.Deserialize<Dictionary<long, string[]>>(json, Options);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.ContainsKey(123L));
        Assert.IsTrue(result.ContainsKey(456L));
        Assert.AreEqual(2, result[123L].Length);
        Assert.AreEqual("tag1", result[123L][0]);
        Assert.AreEqual("tag2", result[123L][1]);
        Assert.AreEqual("tag3", result[456L][0]);
    }

    [TestMethod]
    public void Read_NullToken_ReturnsNull()
    {
        var result = JsonSerializer.Deserialize<Dictionary<long, string[]>>("null", Options);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Write_EmptyDictionary_WritesEmptyArray()
    {
        var dict = new Dictionary<long, string[]>();
        var json = JsonSerializer.Serialize(dict, Options);

        Assert.AreEqual("[]", json);
    }

    [TestMethod]
    public void Write_PopulatedDictionary_WritesObjectWithStringKeys()
    {
        var dict = new Dictionary<long, string[]>
        {
            { 123L, ["tag1", "tag2"] },
            { 456L, ["tag3"] }
        };
        var json = JsonSerializer.Serialize(dict, Options);

        Assert.IsTrue(json.Contains("\"123\""));
        Assert.IsTrue(json.Contains("\"456\""));
        Assert.IsTrue(json.Contains("\"tag1\""));
        Assert.IsTrue(json.Contains("\"tag2\""));
        Assert.IsTrue(json.Contains("\"tag3\""));
    }

    [TestMethod]
    public void RoundTrip_DeserializeSerialize_ProducesSameContent()
    {
        var original = new Dictionary<long, string[]>
        {
            { 100L, ["a", "b"] },
            { 200L, ["c"] },
            { 300L, [] }
        };

        var json = JsonSerializer.Serialize(original, Options);
        var deserialized = JsonSerializer.Deserialize<Dictionary<long, string[]>>(json, Options);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(original.Count, deserialized.Count);
        foreach (var kvp in original)
        {
            Assert.IsTrue(deserialized.ContainsKey(kvp.Key));
            CollectionAssert.AreEqual(kvp.Value, deserialized[kvp.Key]);
        }
    }
}
