using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;
public interface IDamageModifier : IModifierComponent
{
    DamagePhase Phase { get; }
    double Apply(double currentValue, DamageContext ctx);
}