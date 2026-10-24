using FrogBattleV4.Core;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrogBattleV4.Tests.Core;

[TestClass]
[TestSubject(typeof(EntityUid))]
public class EntityUidTest
{

    [TestMethod]
    public void Equals_DifferentInstances_False()
    {
        var a = new EntityUid();
        var b = new EntityUid();
        Assert.AreNotEqual(a, b);
    }

    [TestMethod]
    public void Equals_SameInstance_True()
    {
        var a = new EntityUid();
        Assert.AreEqual(a, a);
    }
}