using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers.Components;

public class PoolValueModifier : StatModifier<PoolStatQuery>
{
    public required PoolId PoolId { get; init; }
    public required PoolValueChannel Channel { get; init; }

    protected override bool AppliesTo(PoolStatQuery query)
    {
        return query.PoolId == PoolId && query.Channel == Channel;
    }
}