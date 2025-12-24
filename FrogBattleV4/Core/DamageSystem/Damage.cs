#nullable enable
using System;
using FrogBattleV4.Core.Pipelines;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.DamageSystem;

public class Damage
{
    private readonly double _baseAmount;

    public required double BaseAmount
    {
        init => _baseAmount = value;
    }
    public required ICharacter? Source { get; init; }
    public required ITargetable Target { get; init; }
    public required Random Random { private get; init; }

    public required DamageProperties Properties { get; init; }
    public double Amount =>
        new DamageCalcContext
        {
            Attacker = Source,
            Target = Target as ICharacter,
            Properties = Properties,
            Type = Properties.Type,
            Source = "idk lmao",
            RawDamage = _baseAmount,
            Rng = Random
        }.ComputePipeline();
}