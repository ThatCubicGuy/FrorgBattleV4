#nullable enable
using System;
using System.Collections.Generic;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.CharacterSystem.Pools;

/// <summary>
/// Standard positive pool component for a character.
/// </summary>
/// <param name="owner">The character who possesses this pool.</param>
public class PoolComponent(BattleMember owner) : IPoolComponent
{
    private double _currentValue;

    public required string Id { get; init; }
    public double CurrentValue
    {
        get => _currentValue;
        set => _currentValue = Math.Clamp(value, 0, Capacity);
    }

    public double Capacity => new PoolValueCalcContext
    {
        Channel = PoolPropertyChannel.Max,
        Actor = owner,
        Other = null,
        PoolId = Id
    }.ComputePipeline(owner.GetStat("Max" + Id));
    public required PoolFlags Flags { get; init; }

    double? IPoolComponent.MaxValue => Capacity;
    double? IPoolComponent.MinValue => 0;
    
    public IEnumerable<IMutatorComponent>? Mutators { get; set; }
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