using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

public class PoolStatModifier : AttributeModifier<PoolStatQuery>
{
    public required PoolName Pool { get; init; }
    public required PoolStatChannel Channel { get; init; }

    protected override bool AppliesTo(PoolStatQuery query)
    {
        return query.Pool == Pool && query.Channel == Channel;
    }
}