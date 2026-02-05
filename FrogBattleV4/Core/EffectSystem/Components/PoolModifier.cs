using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolModifier : IModifierRule<PoolQuery>
{
    public required ModifierStack ModifierStack { get; init; } = new();
    public required string PoolId { get; init; }
    public required PoolPropertyChannel Channel { get; init; }

    public bool AppliesFor(PoolQuery query)
    {
        return query.PoolId == PoolId && query.Channel == Channel;
    }
}