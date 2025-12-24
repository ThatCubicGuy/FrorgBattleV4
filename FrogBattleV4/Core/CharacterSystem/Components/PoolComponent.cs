#nullable enable
using System;
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;

namespace FrogBattleV4.Core.CharacterSystem.Components;

public abstract class PoolComponent(ICharacter owner) : IPoolComponent
{
    private double _currentValue;
    public ICharacter Owner { get; init; } = owner;
    public IEnumerable<IMutatorComponent>? Mutators { get; set; }
    public abstract string Id { get; }

    public double CurrentValue
    {
        get => _currentValue;
        set => _currentValue = Math.Clamp(value, 0, Capacity);
    }
    public double Capacity => Owner.GetStat("Max" + Id);

    public required PoolFlags Flags { get; init; }

    // not sure why lmao
    double? IPoolComponent.MaxValue => Capacity;
}

internal enum Pools
{
    Hp,
    Mana,
    Energy,
    Shield,
    Barrier,
    Special
}