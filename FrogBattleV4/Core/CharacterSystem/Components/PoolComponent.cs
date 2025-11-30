using System;

namespace FrogBattleV4.Core.CharacterSystem.Components;

public abstract class PoolComponent(ICharacter owner) : IPoolComponent
{
    private double _currentValue;

    public ICharacter Owner { get; init; } = owner;
    public abstract string Id { get; }

    public double CurrentValue
    {
        get => _currentValue;
        set => _currentValue = Math.Max(0, Math.Min(value, MaxValue));
    }
    public double MaxValue => Owner.GetStat("Max" + Id);
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