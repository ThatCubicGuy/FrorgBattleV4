using System;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core;
using FrogBattleV4.Core.Selections;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrogBattleV4.Tests.Core;

[TestClass]
[TestSubject(typeof(BattleManager.Builder))]
public class BattleManagerBuilderTest
{
    private static readonly ISelectionProvider FakeProvider = new FakeSelectionProvider();

    [TestMethod]
    public void AddTeam_WhenPreviousContainsNoMembers_ThrowsInvalidOperationException()
    {
        var builder = BattleManager.GetBuilder().AddTeam(FakeProvider);

        Assert.Throws<InvalidOperationException>(() => builder.AddTeam(FakeProvider));
    }

    [TestMethod]
    public void AddMember_WhenNoPreviousTeam_ThrowsInvalidOperationException()
    {
        var member = default(FighterId);
        var builder = BattleManager.GetBuilder();

        Assert.Throws<InvalidOperationException>(() => builder.AddMember(member));
    }

    [TestMethod]
    public void Build_WhenNotEnoughTeams_ThrowsInvalidOperationException()
    {
        var noTeamsBuilder = BattleManager.GetBuilder();
        var oneTeamBuilder = BattleManager.GetBuilder().AddTeam(FakeProvider);

        Assert.Throws<InvalidOperationException>(noTeamsBuilder.Build);
        Assert.Throws<InvalidOperationException>(oneTeamBuilder.Build);
    }

    [TestMethod]
    public void Build_WhenConditionsMet_WorksAsExpected()
    {
        var member = default(FighterId);
        var builder = BattleManager.GetBuilder();
        // Team 1
        builder.AddTeam(FakeProvider).AddMember(member);
        // Team 2
        builder.AddTeam(FakeProvider).AddMember(member);

        builder.Build();
    }

    private class FakeSelectionProvider : ISelectionProvider
    {
        public Task<PlayerTurnAction> GetSelectionAsync(PlayerActionRequest request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}