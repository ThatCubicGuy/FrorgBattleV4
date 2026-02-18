using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolValueModifier : ModifierRule<PoolValueQuery>
{
    public required PoolId PoolId { get; init; }
    public required PoolValueChannel Channel { get; init; }

    protected override bool AppliesToQuery(PoolValueQuery query)
    {
        return query.PoolId == PoolId && query.Channel == Channel;
    }
}