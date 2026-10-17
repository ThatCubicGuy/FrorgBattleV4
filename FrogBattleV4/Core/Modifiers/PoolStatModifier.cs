using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

public class PoolStatModifier : AttributeModifier<PoolStatQuery>
{
    public required PoolId PoolId { get; init; }
    public required PoolStatChannel Channel { get; init; }

    protected override bool AppliesTo(PoolStatQuery query)
    {
        return query.PoolId == PoolId && query.Channel == Channel;
    }
}