#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.CharacterSystem.Components;

/// <summary>
/// Standard positive pool component for a character.
/// </summary>
/// <param name="owner">The character who possesses this pool.</param>
public class PoolComponent(ICharacter owner) : IPoolComponent
{
    private double _currentValue;

    public required string Id { get; init; }
    public double CurrentValue
    {
        get => _currentValue;
        init => _currentValue = Math.Clamp(0, value, Capacity);
    }

    public double Capacity => new PoolCalcContext
    {
        Channel = PoolModType.Max,
        Owner = Owner,
        PoolId = Id
    }.ComputePipeline(Owner.GetStat("Max" + Id));
    public required PoolFlags Flags { get; init; }
    public ICharacter Owner { get; init; } = owner;
    public IEnumerable<IMutatorComponent>? Mutators { get; set; }

    // not sure why lmao
    double? IPoolComponent.MaxValue => Capacity;
    double? IPoolComponent.MinValue => 0;

    public SpendResult ProcessSpend(double amount, SpendContext ctx)
    {
        var calcCtx = new PoolCalcContext
        {
            Channel = PoolModType.Cost,
            Owner = ctx.Owner,
            PoolId = Id
        };
        switch (ctx.Mode)
        {
            case SpendMode.Preview:
                return new SpendResult(calcCtx.ComputePipeline(amount), true);
                break;
            case SpendMode.Validate:
                return new SpendResult(0, calcCtx.ComputePipeline(amount) <= CurrentValue);
                break;
            case SpendMode.Commit:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ctx), ctx.Mode, "Spend mode is not supported.");
        }
        throw new NotImplementedException();
    }

    public double ProcessRegen(double amount, ICharacter character)
    {
        throw new NotImplementedException();
    }
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