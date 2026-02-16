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
    // TODO: Fix! Currently you only allow mutations through PoolPipeline.
    // TODO: See how u might solve this one.
    public double Capacity => new ModifierContext(owner).ResolveMore(new StatQuery
    {
        Stat = "Max" + Id,
    }, out var statStack).Resolve(new PoolValueQuery
    {
        Channel = PoolPropertyChannel.Max,
        PoolId = Id
    }).ApplyTo(statStack.ApplyTo(owner.BaseStats["Max" + Id]));

    public override double? MaxValue => Capacity;
    public override double? MinValue => 0;
}