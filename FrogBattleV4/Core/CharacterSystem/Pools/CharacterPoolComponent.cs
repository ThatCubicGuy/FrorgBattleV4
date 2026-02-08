#nullable enable
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.CharacterSystem.Pools;

/// <summary>
/// Standard positive pool component for a character.
/// </summary>
/// <param name="owner">The character who possesses this pool.</param>
public class CharacterPoolComponent(BattleMember owner) : PoolComponent
{
    // TODO: Fix! Currently you only allow mutations through PoolPipeline.
    // TODO: See how u might solve this one.
    public double Capacity => new PoolValueCalcContext
    {
        Channel = PoolPropertyChannel.Max,
        Holder = owner,
        Other = null,
        PoolId = Id
    }.ComputePipeline(owner.GetStat("Max" + Id));

    public override double? MaxValue => Capacity;
    public override double? MinValue => 0;
}

internal enum Pool
{
    Hp,
    Mana,
    Energy,
    Shield,
    Barrier,
    Special
}