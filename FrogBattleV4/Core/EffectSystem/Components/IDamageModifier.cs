#nullable enable
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;
public interface IDamageModifier : IModifierComponent
{
    string? Type { get; }
    string? Source { get; }
    DamagePhase Phase { get; }
    double Apply(double currentValue, DamageContext ctx);
}