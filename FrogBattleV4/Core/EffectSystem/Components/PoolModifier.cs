using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolModifier : ModifierRule<PoolQuery>
{
    public required PoolId PoolId { get; init; }
    /// <summary>
    /// <p>Channel for the pool modification, e.g. max value, cost, regen.</p>
    /// <p>NOT whether this modifier is incoming/outgoing.</p>
    /// </summary>
    public required PoolPropertyChannel Channel { get; init; }

    protected override bool AppliesToRequest(PoolQuery query)
    {
        return query.PoolId == PoolId && query.Channel == Channel;
    }
}