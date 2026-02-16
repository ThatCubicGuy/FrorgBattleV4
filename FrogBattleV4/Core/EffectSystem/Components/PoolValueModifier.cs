using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.Pipelines;

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