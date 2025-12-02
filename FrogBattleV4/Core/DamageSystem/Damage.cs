#nullable enable
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.DamageSystem;

public class Damage
{
    private readonly double _baseAmount;
    [SetsRequiredMembers]
    public Damage(ICharacter source, ITargetable target, double amount, DamageProperties properties)
    {
        Source = source;
        Target = target;
        _baseAmount = amount;
        Properties = properties;
    }
    public required ICharacter? Source { get; init; }
    public required ITargetable Target { get; init; }

    public DamageProperties Properties { get; init; }
    public double Amount =>
        DamagePipeline.ComputePipeline(new DamageContext
        {
            Attacker = Source,
            Target = Target as ICharacter,
            DamageSource = "idk lmao",
            Properties = Properties,
            RawDamage = _baseAmount,
            // Rng = [uh oh]
        });
}