#nullable enable
using FrogBattleV4.Core.Pipelines;
using FrogBattleV4.Core.Pipelines.Pools;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// Standard positive pool component for a character.
/// </summary>
/// <param name="owner">The character who possesses this pool.</param>
public class CharacterPoolComponent(Character owner) : PoolComponent
{
    public double Capacity => new PoolValueQuery
    {
        Channel = PoolValueChannel.Max,
        PoolId = Id
    }.Compute(new ModifierContext(owner)
        .ComputeStat("max_" + Id), new ModifierContext(owner));

    public override double? MaxValue => Capacity;
    public override double? MinValue => 0;
}