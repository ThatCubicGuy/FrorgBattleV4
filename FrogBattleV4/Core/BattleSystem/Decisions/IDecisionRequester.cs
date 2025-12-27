using System.Collections;
using System.Collections.Generic;

namespace FrogBattleV4.Core.BattleSystem.Decisions;

public interface IDecisionRequester<out T> : IDecisionRequester where T : notnull
{
    public new IEnumerable<T> GetOptions(BattleContext ctx);
    IEnumerable IDecisionRequester.GetOptions(BattleContext ctx) => GetOptions(ctx);
}

public interface IDecisionRequester
{
    public IEnumerable GetOptions(BattleContext ctx);
}