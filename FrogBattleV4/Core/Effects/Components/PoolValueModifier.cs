using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Effects.Modifiers;

namespace FrogBattleV4.Core.Effects.Components;

public class PoolValueModifier : ModifierRule<PoolValueQuery>
{
    public required PoolId PoolId { get; init; }
    public required PoolValueChannel Channel { get; init; }

    protected override bool AppliesToQuery(PoolValueQuery query)
    {
        return query.PoolId == PoolId && query.Channel == Channel;
    }
}