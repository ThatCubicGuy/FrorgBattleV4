using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

/// <summary>
/// Represents an object that modifies the value of a pool at certain points in time.
/// </summary>
public interface IMutatorComponent
{
    void Tick(TurnMoment moment, StatusEffectInstance effect, CommandBuilder builder);
}