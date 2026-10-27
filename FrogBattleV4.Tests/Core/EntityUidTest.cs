using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrogBattleV4.Tests.Core;

public readonly record struct Uid<T>()
{
    // Intentionally different for each generated type
    // ReSharper disable once StaticMemberInGenericType
    private static long _idCounter;
    private long Id { get; } = checked(++_idCounter);
    public override string ToString() => $"TeamUid #{Id}";
}

[TestClass]
[TestSubject(typeof(Uid<>))]
public class UidTest
{

    [TestMethod]
    public void Equals_DifferentInstances_False()
    {
        var a = new Uid<int>();
        var b = new Uid<int>();
        Assert.AreNotEqual(a, b);
    }

    [TestMethod]
    public void Equals_SameInstance_True()
    {
        var a = new Uid<int>();
        Assert.AreEqual(a, a);
    }
}