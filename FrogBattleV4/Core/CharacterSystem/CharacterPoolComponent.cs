#nullable enable
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// Standard positive pool component for a character.
/// </summary>
/// <param name="owner">The character who possesses this pool.</param>
public class CharacterPoolComponent(IBattleMember owner) : PoolComponent
{
    // Context for calculating capacity is always identical
    private readonly ModifierContext _ctx = new(owner);

    public double Capacity => new PoolValueQuery
    {
        Channel = PoolValueChannel.Max,
        PoolId = Id
    }.Compute(_ctx.ComputeStat("max_" + Id), _ctx);

    public override double? MaxValue => Capacity;
    public override double? MinValue => 0;
}