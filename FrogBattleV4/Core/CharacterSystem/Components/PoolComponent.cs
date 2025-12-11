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
        set => _currentValue = Math.Max(0, Math.Min(value, Capacity));
    }
    /// <summary>
    /// The maximum value that this pool component can have.
    /// </summary>
    public virtual double Capacity => Owner.GetStat("Max" + Id);
}

internal enum Pools
{
    Hp,
    Mana,
    Energy,
    Special,
    Shield,
    Barrier
}