using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;
/// <summary>
/// Damage can only be modified on a percentage basis.
/// </summary>
public interface IDamageModifier : IModifierComponent
{
    bool CanApply(DamagePhase phase, DamageContext ctx);
}