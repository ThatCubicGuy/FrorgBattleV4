using System.Text.Json;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.JsonDeserialization;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrogBattleV4.Tests.JsonDeserialization;

[TestClass]
[TestSubject(typeof(ExpandableEnumConverter<>))]
public class ExpandableEnumConverterTest
{
    private const string Name = "something_i_guess";

    [TestMethod]
    public void Deserialize_WhenString_Converts()
    {
        const string jsonName = $"""
                                 "{Name}"
                                 """;
        var statName = new StatName(Name);
        var poolName = new PoolName(Name);
        var statNameDeserialized = JsonSerializer.Deserialize(jsonName, AppSerializerContext.Default.StatName);
        var poolNameDeserialized = JsonSerializer.Deserialize(jsonName, AppSerializerContext.Default.PoolName);
        Assert.AreEqual(statNameDeserialized, statName);
        Assert.AreEqual(poolNameDeserialized, poolName);
    }

    [TestMethod]
    public void Deserialize_WhenNotString_ThrowsError()
    {
        var invalidJson = $$"""
                            {
                              "name": "{{Name}}"
                            }
                            """;
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize(invalidJson, AppSerializerContext.Default.StatName));
    }

    [TestMethod]
    public void Deserialize_WhenInvalidName_ThrowsError()
    {
        var invalidName = """
                          "This is a VERY !!! invalid name :3"
                          """;
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize(invalidName, AppSerializerContext.Default.StatName));
    }
}