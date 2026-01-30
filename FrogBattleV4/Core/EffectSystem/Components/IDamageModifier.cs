#nullable enable
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;
public interface IDamageModifier : IModifierComponent<DamageCalcContext>
{
    string? Type { get; }
    string? Source { get; }
}