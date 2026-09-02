using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

public class PoolMutModifier : MutationModifier<MutateQuery>
{
    public required MutateData Data { get; init; }
    /// <summary>
    /// <p>Channel for the pool modification, e.g. cost, regen.</p>
    /// <p>NOT whether this modifier is incoming/outgoing.</p>
    /// </summary>
    public required PoolMutChannel Channel { get; init; }

    protected override bool AppliesTo(MutateQuery query)
    {
        return query.Data == Data;
    }
}